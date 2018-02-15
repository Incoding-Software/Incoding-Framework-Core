using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Incoding.Core.Block.Logging;
using Incoding.Core.Block.Logging.Core;

namespace Incoding.Core.Block.Caching
{
    public class FileCaching
    {
        public static TInstance Evaluate<TInstance>(string path, Func<TInstance> eval) where TInstance : class
        {
            TInstance instance = default(TInstance);

            IFormatter serializer = new BinaryFormatter();
            if (File.Exists(path))
            {
                try
                {
                    using (Stream stream = File.OpenRead(path))
                        instance = serializer.Deserialize(stream) as TInstance;
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
                    using (Stream stream = File.OpenWrite(path))
                        serializer.Serialize(stream, instance);
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