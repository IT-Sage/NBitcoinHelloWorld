using NBitcoin;
using System;

namespace NBitcoinHelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            SignUsingBuilder();
            //SignTransaction();
        }

        private static void SignUsingBuilder()
        {
            var secret = new BitcoinSecret("cSmnhYYG2mXRPvi1FoFDihT4bL5qy9DDhephoubJ7mwxb2sgLNGQ", Network.TestNet);
            BitcoinAddress fromAddressFromSecret = secret.GetAddress(ScriptPubKeyType.Legacy);
            BitcoinAddress fromAddress = BitcoinAddress.Create("mfk4BVNg4p4m7qPx3u398otHT97M9hotPR", Network.TestNet);
            BitcoinAddress toAddress = BitcoinAddress.Create("mkQporSV7myJLwfWEVyHMhphY9viiiEMWc", Network.TestNet);

            Transaction previousTransaction = Transaction.Create(Network.TestNet);
            previousTransaction.Outputs.Add(new TxOut(new Money(0.0001m, MoneyUnit.BTC), fromAddress));


            var coinsToSpend = previousTransaction.Outputs.AsCoins();

            TransactionBuilder transactionBuilder = Network.TestNet.CreateTransactionBuilder();

            var tx = transactionBuilder
                    .AddCoins(coinsToSpend)
                    .AddKeys(secret)
                    .Send(toAddress, Money.Coins(0.00009m))
                    .SendFees(Money.Coins(0.00001m))
                    .SetChange(fromAddress)
                    .BuildTransaction(true);

            var check = transactionBuilder.Check(tx);

            if (transactionBuilder.Verify(tx, out var errors))
            {
                Console.WriteLine("COME ON!");
            }

        }

        private static void SignTransaction()
        {
            string privateKey = "cSmnhYYG2mXRPvi1FoFDihT4bL5qy9DDhephoubJ7mwxb2sgLNGQ";
            var secret = new BitcoinSecret(privateKey, Network.TestNet);

            var fromAddress = BitcoinAddress.Create("mfk4BVNg4p4m7qPx3u398otHT97M9hotPR", Network.TestNet); // == secret.GetAddress();
            Script scriptSigFromAddress = fromAddress.ScriptPubKey;
            Script scriptSigFromSecret = secret.GetAddress(ScriptPubKeyType.Legacy).ScriptPubKey;

            var utxoSignature = "76a914027a52530751c0165299b9b8318746c3739e730388ac";
            Script scriptSig = new Script(utxoSignature);


            string txHash = "581e53621313e977be64deb1b1afa97f000e5c9f352642341837e63733014af0";
            uint256 transactionId = uint256.Parse(txHash);
            var previousOutpoint = new OutPoint(transactionId, 0);

            Transaction currentTransaction = Network.TestNet.CreateTransaction();

            TxIn txInput = new TxIn(previousOutpoint, scriptSigFromAddress); //need to prove you own this input
            currentTransaction.Inputs.Add(txInput);

            var toAddress = BitcoinAddress.Create("mkQporSV7myJLwfWEVyHMhphY9viiiEMWc", Network.TestNet);
            Money fee = Money.Satoshis(1000);
            var money = new Money(0.0001M, MoneyUnit.BTC) - fee;
            var txOut = new TxOut(money, toAddress.ScriptPubKey);
            currentTransaction.Outputs.Add(txOut);

            currentTransaction.Sign(secret, new Coin(transactionId, 0, new Money(0.0001M, MoneyUnit.BTC), scriptSigFromAddress));
            Console.WriteLine(currentTransaction.ToHex());


        }

        private static void TestMethod()
        {
            Key privateKey = new Key();
            PubKey publicKey = privateKey.PubKey;

            string mnemonicString = "quantum tobacco key they maid mean crime youth chief jungle mind design broken tilt bus shoulder leaf good forward erupt split divert bread kitten";
            var mnemonic = new Mnemonic(mnemonicString);
            var seed = mnemonic.DeriveSeed();

            var xprv = mnemonic.DeriveExtKey().Derive(new KeyPath("m/44\'/0\'/0\'/0"));

            var xpub = xprv.Neuter();
            Console.WriteLine("xpub: " + xpub.ToString(Network.Main));

            Console.WriteLine("Private Key Test net: " + privateKey.ToString(Network.TestNet));
            Console.WriteLine("Secret Test net" + privateKey.GetBitcoinSecret(Network.TestNet).ToString());
            //Console.WriteLine("address Main net: " + publicKey.GetAddress(ScriptPubKeyType.Legacy, Network.Main));
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
