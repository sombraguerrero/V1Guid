using System.Runtime.InteropServices;
[DllImport("rpcrt4.dll", SetLastError = true)] //DLL still present as of Windows 11 - both architectures
static extern int UuidCreateSequential(out Guid guid); //Function names are case-sensitive per the output of dumpbin /exports 

const int RPC_S_OK = 0;
int num;
Guid g;
if (args.Length == 1 && int.TryParse(args[0], out num))
{
    for (int i = 0; i < num; i++)
    {
        if (UuidCreateSequential(out g) == RPC_S_OK)
        {
            Console.WriteLine(g.ToString().ToUpper());
        }
        else
            Console.WriteLine("Creation of this GUID failed!");
    }
}
else if (args.Length == 0)
{
    Console.Write("Number of Guids: ");
    if (int.TryParse(Console.ReadLine(), out num))
    {
        for (int i = 0; i < num; i++)
        {
            if (UuidCreateSequential(out g) == RPC_S_OK)
            {
                Console.WriteLine(g.ToString().ToUpper());
            }
            else
                Console.WriteLine("Creation of this GUID failed!");
        }
    }
}

else if (UuidCreateSequential(out g) == RPC_S_OK)
{
    Console.WriteLine(g.ToString().ToUpper());
}
else
{
    Console.WriteLine("No quantity provided and single GUID generation failed!");
}
Console.Write("Press any key to continue...");
_ = Console.ReadKey();