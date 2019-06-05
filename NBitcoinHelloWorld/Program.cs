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
            Console.WriteLine("address Main net: " + publicKey.GetAddress(ScriptPubKeyType.Legacy, Network.Main));
            Console.WriteLine("address Test net: " + publicKey.GetAddress(ScriptPubKeyType.Legacy, Network.TestNet));
            Console.WriteLine("Segwit address Test net: " + publicKey.GetAddress(ScriptPubKeyType.Segwit, Network.TestNet));

            var publicKeyHash = publicKey.Hash;
            Console.WriteLine("public key hash:" + publicKeyHash);
            var mainNetAddress = publicKeyHash.GetAddress(Network.Main);
            var testNetAddress = publicKeyHash.GetAddress(Network.TestNet);
            Console.WriteLine("address Main net: " + mainNetAddress);
            Console.WriteLine("address Test net: " + testNetAddress);

            Console.WriteLine("----------------------");
            var publicKeyHash2 = new KeyId("14836dbe7f38c5ac3d49e8d790af808a4ee9edcf");

            var testNetAddress2 = publicKeyHash2.GetAddress(Network.TestNet);
            var mainNetAddress2 = publicKeyHash2.GetAddress(Network.Main);

            Console.WriteLine(mainNetAddress2.ScriptPubKey); // OP_DUP OP_HASH160 14836dbe7f38c5ac3d49e8d790af808a4ee9edcf OP_EQUALVERIFY OP_CHECKSIG
            Console.WriteLine(testNetAddress2.ScriptPubKey); // OP_DUP OP_HASH160 14836dbe7f38c5ac3d49e8d790af808a4ee9edcf OP_EQUALVERIFY OP_CHECKSIG 

            Console.WriteLine("-----------------------");
            var paymentScript = publicKeyHash.ScriptPubKey;
            var sameMainNetAddress = paymentScript.GetDestinationAddress(Network.Main);
            Console.WriteLine(mainNetAddress == sameMainNetAddress); // True

        }
    }
}
