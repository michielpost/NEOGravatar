using Neo.Cryptography;
using Neo.VM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NeoGravatar.Contracts.Tests
{
  class Program
  {
    static void Main(string[] args)
    {
      var engine = new ExecutionEngine(null, Crypto.Default);
      engine.LoadScript(File.ReadAllBytes(@"C:\Source\NeoGravatar\src\NeoGravatar.Contracts\bin\Debug\NeoGravatar.Contracts.avm"));

      using (ScriptBuilder sb = new ScriptBuilder())
      {
        //sb.EmitPush(new object[] { "first", "second"}); // corresponds to the parameter b
        sb.EmitPush("2"); // corresponds to the parameter a
        sb.EmitPush("1"); // corresponds to the parameter a
        sb.EmitPush(2);
        sb.Emit(OpCode.PACK);
        sb.EmitPush("debug"); // corresponds to the parameter a
        engine.LoadScript(sb.ToArray());
      }

      engine.Execute(); // start execution

      var result = engine.EvaluationStack.Peek().GetString(); // set the return value here
      Console.WriteLine($"Execution result {result}");
      Console.ReadLine();

    }

    public static byte[] ObjectToByteArray(Object obj)
    {
      BinaryFormatter bf = new BinaryFormatter();
      using (var ms = new MemoryStream())
      {
        bf.Serialize(ms, obj);
        return ms.ToArray();
      }
    }
  }
}
