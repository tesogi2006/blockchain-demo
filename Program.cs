using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace blockchain_demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var blockChain = new BlockChain();
            var block1 = new Block{ Id = 1, Data = "{ balance: 20 }" };
            blockChain.AddBlock(block1);

            var block2 = new Block{ Id = 2, Data = "{ balance: 150 }" };
            blockChain.AddBlock(block2);
            blockChain.Print();            
            Console.WriteLine("Valid block? " + blockChain.IsValid());

            // now tamper with data
            blockChain.Blocks[1].Data = "{ balance: 151 }";
            blockChain.Print();
            Console.WriteLine("Valid block? " + blockChain.IsValid());
        }
    }
}
