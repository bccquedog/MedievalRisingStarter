using System;
using System.IO;
using MedievalRising.Application.Persistence;
using MedievalRising.Domain.World;
using UnityEngine;

namespace MedievalRising.Infrastructure.Persistence
{
    public sealed class LocalJsonSaveRepository : ISaveRepository
    {
        private readonly string _root;

        public LocalJsonSaveRepository(string root = null)
        {
            _root = root ?? Path.Combine(UnityEngine.Application.persistentDataPath, "Saves");
        }

        public bool Exists(string slotId) => File.Exists(GetPath(slotId));

        public void Save(string slotId, WorldState world)
        {
            Directory.CreateDirectory(_root);
            string destination = GetPath(slotId);
            string temporary = destination + ".tmp";
            string backup = destination + ".bak";
            string json = JsonUtility.ToJson(SaveMapper.ToDto(world), true);

            File.WriteAllText(temporary, json);
            if (File.Exists(destination))
            {
                File.Copy(destination, backup, true);
            }

            File.Copy(temporary, destination, true);
            File.Delete(temporary);
        }

        public WorldState Load(string slotId)
        {
            string path = GetPath(slotId);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Save slot does not exist: {slotId}", path);
            }

            try
            {
                return ReadWorld(path);
            }
            catch (Exception primaryFailure)
            {
                string backup = path + ".bak";
                if (!File.Exists(backup))
                {
                    throw new InvalidDataException("Save could not be read and no backup exists.", primaryFailure);
                }

                try
                {
                    return ReadWorld(backup);
                }
                catch (Exception backupFailure)
                {
                    throw new AggregateException("Both the current save and its backup are unreadable.", primaryFailure, backupFailure);
                }
            }
        }

        private string GetPath(string slotId)
        {
            foreach (char invalid in Path.GetInvalidFileNameChars())
            {
                if (slotId.IndexOf(invalid) >= 0)
                {
                    throw new ArgumentException("Save slot contains invalid characters.", nameof(slotId));
                }
            }

            return Path.Combine(_root, slotId + ".json");
        }

        private static WorldState ReadWorld(string path)
        {
            SaveGameDto dto = JsonUtility.FromJson<SaveGameDto>(File.ReadAllText(path));
            return SaveMapper.FromDto(dto);
        }
    }
}
