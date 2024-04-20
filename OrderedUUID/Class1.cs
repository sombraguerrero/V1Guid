using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System;

namespace OrderedUUID
{
    public class Class1
    {
        [DllImport("rpcrt4.dll", SetLastError = true)] //DLL still present as of Windows 11 - both architectures
        static extern int UuidCreateSequential(out Guid guid); //Function names are case-sensitive per the output of dumpbin /exports 
        const int RPC_S_OK = 0;

        public static void CreateV1Guid(out byte[] newUUID)
        {
            Guid g;
            if (UuidCreateSequential(out g) == RPC_S_OK)
            {
                string gString = g.ToString().ToUpper();
                string gSubString = gString.Substring(14, 4) + gString.Substring(9, 4) + gString.Substring(0, 8) + gString.Substring(19, 4) + gString.Substring(24);
                newUUID = Encoding.Default.GetBytes(gSubString);
            }
            else
            {
                newUUID = Array.Empty<byte>();
            }
        }

        [SqlFunction(FillRowMethodName = "FillRow")]
        public static IEnumerable InitMethod(int num = 1)
        {
            ArrayList arrayList = new ArrayList();
            byte[] myGuid;
            for (int i = 1; i <= num; i++)
            {
                CreateV1Guid(out myGuid);
                arrayList.Add(myGuid);
            }
            return arrayList;
        }

        public static void FillRow(object obj, out SqlBinary binary)
        {
            if (obj != null)
            {
                byte[] buffer = obj as byte[];
                binary = new SqlBinary(buffer);
            }
            else
            {
                binary = Array.Empty<byte>();
            }
        }
    }
}
