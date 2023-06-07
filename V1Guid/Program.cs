using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

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
    string gString = g.ToString().ToUpper();
    string gSubString = gString.Substring(14, 4) + gString.Substring(9, 4) + gString.Substring(0, 8) + gString.Substring(19, 4) + gString.Substring(24);
    byte[] gSubStringBytes = Encoding.Default.GetBytes(gSubString);
    Console.WriteLine("Original: {0}\r\nSubstringed: {1}\r\nHex: {2}\r\nVia BitConverter: {3}", gString, gSubString, Convert.ToHexString(gSubStringBytes), BitConverter.ToString(gSubStringBytes).Replace("-", string.Empty));
}
else
{
    Console.WriteLine("No quantity provided and single GUID generation failed!");
}
Console.Write("Press any key to continue...");
_ = Console.ReadKey();