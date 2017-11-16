using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sparrow.ResourceLoading;
using Sparrow.Textures;
using Stardust.Actions;
using Stardust.Emitters;
using Stardust.Sparrow.Player.Emitters;
using Stardust.Sparrow.Player.Project;
using Stardust.Serialization;

namespace Stardust.Sparrow.Player
{
    public class SimLoader
    {
        private IList<RawEmitterData> _rawEmitterDatas = new List<RawEmitterData>();
        private TextureAtlas _atlas;
        private bool _projectLoaded;
        private JObject _descJson;
        private StardustSerializer _stardusSerializer = new StardustSerializer();
        
        /// <summary>
        /// Loads a simulation
        /// </summary>
        public void LoadSim(Stream stream)
        {
            ValidateSde(stream);
            LoadSde(stream);
        }

        private void LoadSde(Stream stream)
        {
            ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read);
            _descJson = JObject.Parse(EntryToString(zip.GetEntry(SDEConstants.DESCRIPTOR_FILENAME)));
            
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                if (SDEConstants.IsEmitterXmlName(entry.Name))
                {
                    string emitterId = SDEConstants.GetEmitterId(entry.Name);
                    RawEmitterData rawData = new RawEmitterData();
                    rawData.EmitterId = emitterId;
                    rawData.EmitterString = EntryToString(entry);
                    // + parse snapshot
                    _rawEmitterDatas.Add(rawData);
                }
            }
            // texture atlas
            var atlasZipEntry = zip.GetEntry(SDEConstants.ATLAS_XML_NAME);
            string atlasXmlString = EntryToString(atlasZipEntry);
            
            var atlasImageEntry = zip.GetEntry(SDEConstants.ATLAS_IMAGE_NAME);
            var texLoader = new TextureLoader();
            Texture tex = texLoader.LoadFromStream(atlasImageEntry.Open());
            
            _atlas = new TextureAtlas(atlasXmlString, tex);
            
            _projectLoaded = true;
        }

        public ProjectValueObject CreateProjectInstance()
        {
            if (!_projectLoaded)
            {
                throw new InvalidOperationException("No project is loaded, call LoadSim() first!");
            }
            float version = _descJson["version"].Value<float>();
            var project = new ProjectValueObject(version);
            foreach (RawEmitterData rawData in _rawEmitterDatas)
            {
                Emitter emitter = _stardusSerializer.Deserialize(rawData.EmitterString);
                emitter.Name = rawData.EmitterId;
                //REMOVE Emitter2D emitter = EmitterBuilder.BuidEmitter(rawData.EmitterString, rawData.EmitterId);
                EmitterValueObject emitterVo = new EmitterValueObject(emitter);
                project.Emitters.Add(emitterVo);
                if (rawData.Snapshot != null)
                {
                    // not done yet
                }
                var textures = _atlas.GetTextures(SDEConstants.GetSubtexturePrefix(emitterVo.Id));
                var subTextures = textures.Cast<SubTexture>().ToList();
                ((StarlingHandler) (emitterVo.Emitter.ParticleHandler)).Textures = subTextures;
            }

            foreach (var emitterVo in project.Emitters)
            {
                foreach (var action in emitterVo.Emitter.Actions)
                {
                    Spawn spawn = action as Spawn;
                    if (spawn?.SpawnerEmitterId != null)
                    {
                        foreach (var emitterValueObject in project.Emitters)
                        {
                            if (spawn.SpawnerEmitterId == emitterValueObject.Id)
                            {
                                spawn.SpawnerEmitter = emitterValueObject.Emitter;
                            }
                        }
                    }
                }
            }
            return project;
        }
        
        /// <summary>
        /// Call this if you don't want to create more instances of this project to free up its memory and
        /// there are no simulations from this loader running.
        /// Note that this disposes the underlying texture atlas!
        /// After calling it createProjectInstance() will not work.
        /// </summary>
        public void Dispose()
        {
            _projectLoaded = false;
            _descJson = null;
            if (_atlas != null)
            {
                _atlas.Dispose();
            }
            foreach (var rawEmitterData in _rawEmitterDatas)
            {
                rawEmitterData.Snapshot = null;
            }
            _rawEmitterDatas = new List<RawEmitterData>();
        }

        private void ValidateSde(Stream stream)
        {
            if (stream == null)
            {
                throw new NullReferenceException("stream cannot be null");
            }
            ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read);
            var desc = zip.GetEntry(SDEConstants.DESCRIPTOR_FILENAME);
            if (desc == null)
            {
                throw new Exception(SDEConstants.DESCRIPTOR_FILENAME + " not found");
            }
            var descJson = JObject.Parse(EntryToString(desc));
            if (descJson["version"].Value<float>() < StardustInfo.Version)
            {
                Debug.WriteLine("Stardust Sim Loader: WARNING loaded simulation is created with an old " +
                                "version of the editor, it might not run.");
            }

            if (zip.GetEntry(SDEConstants.ATLAS_IMAGE_NAME) == null)
            {
                throw new Exception(SDEConstants.ATLAS_IMAGE_NAME + " not found");
            }
            if (zip.GetEntry(SDEConstants.ATLAS_XML_NAME) == null)
            {
                throw new Exception(SDEConstants.ATLAS_XML_NAME + " not found");
            }
            bool emitterXmlFound = false;
            foreach (var entry in zip.Entries)
            {
                if (SDEConstants.IsEmitterXmlName(entry.Name))
                {
                    emitterXmlFound = true;
                }
            }
            if (!emitterXmlFound)
            {
                throw new Exception("Emitter descriptor XML not found");
            }
        }

        private static string EntryToString(ZipArchiveEntry entry)
        {
            StreamReader reader = new StreamReader(entry.Open());
            return reader.ReadToEnd();
        }
    }

    internal class RawEmitterData
    {
        public string EmitterId;
        public string EmitterString;
        public object Snapshot; // TODO
    }
}