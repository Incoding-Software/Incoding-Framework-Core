using System;
using System.IO;
using Incoding.Core.Block.Logging;
using Incoding.Core.Block.Logging.Core;
using ProtoBuf;

namespace Incoding.Core.Block.Caching
{
    public class FileCaching
    {
        public static TInstance Evaluate<TInstance>(string path, Func<TInstance> eval) where TInstance : class
        {
            TInstance instance = default(TInstance);
            
            if (File.Exists(path))
            {
                try
                {
                    using (FileStream stream = File.OpenRead(path))
                        instance = Serializer.Deserialize<TInstance>(stream);
                }
                catch (Exception exLoad)
                {
                    LoggingFactory.Instance.LogException(LogType.Trace, exLoad);
                }
            }
            if (instance == null)
            {
                instance = eval();

                try
                {
                    using (FileStream stream = File.Open(path, FileMode.Create))
                        Serializer.Serialize(stream, instance);
                }
                catch (Exception exSave)
                {
                    LoggingFactory.Instance.LogException(LogType.Trace, exSave);
                }
            }
            return instance;
        }
    }
}