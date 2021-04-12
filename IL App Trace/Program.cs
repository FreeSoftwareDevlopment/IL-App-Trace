using System;
using Mono.Cecil;
using System.IO;
using Mono.Cecil.Cil;
using System.Linq;

namespace IL_App_Trace
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sharkbyteprojects Assembly Tracer\n");
            if (args.Length > 0)
            {
                foreach (string filename in args)
                {
                    if (File.Exists(filename))
                    {
                        Console.WriteLine($"Modify \"{filename}\"");
                        ModuleDefinition module = ModuleDefinition.ReadModule(filename);
                        if (module.Kind == ModuleKind.Windows)
                            module.Kind = ModuleKind.Console;
                        MethodReference clog = module.ImportReference(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(object) }));
                        Instruction loginst = Instruction.Create(OpCodes.Call, clog);
                        foreach (var type in module.Types)
                        {
                            foreach (var method in type.Methods)
                            {
                                if (!method.HasBody) continue;
                                var ilProc = method.Body.GetILProcessor();
                                var fi = method.Body.Instructions.First();
                                var li = method.Body.Instructions.Last();
                                ilProc.InsertBefore(fi, Instruction.Create(OpCodes.Ldstr, $"-- Entering {method.Name} of {type.Name}"));
                                ilProc.InsertBefore(fi, loginst);
                                ilProc.InsertBefore(li, Instruction.Create(OpCodes.Ldstr, $"-- Exiting {method.Name} of {type.Name}"));
                                ilProc.InsertBefore(li, loginst);
                            }
                        }
                        string saveas = string.Format("{0}.trace{1}", Path.GetFileNameWithoutExtension(filename), Path.GetExtension(filename));
                        Console.WriteLine($"Save File: {saveas}");
                        module.Write(saveas);
                    }
                }
                Console.WriteLine("Completed");
            }
            else
            {
                Console.WriteLine("Give as Args IL Assembly Files!");
                Console.Beep();
            }
        }
    }
}
