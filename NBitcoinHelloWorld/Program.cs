using NBitcoin;
using System;

namespace NBitcoinHelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Key privateKey = new Key();
            PubKey publicKey = privateKey.PubKey;
            Console.WriteLine(publicKey.GetAddress(ScriptPubKeyType.Legacy,Network.Main));            
            Console.WriteLine(publicKey.GetAddress(ScriptPubKeyType.Segwit,Network.TestNet));
        }
    }
}
