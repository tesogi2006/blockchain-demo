using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace blockchain_demo
{
    
    class Block{
        public int Id { get; set; }
        public DateTime Date { get;set; }
        public string Data { get; set; }
        public string PrevHash { get; set; }
        public int Nonce { get; set; }
        public string Hash 
        {
            set
            {
                value = CalculateHash();
            }
            get{
                return CalculateHash();
            }
        }
        
        public string CalculateHash()
        {
            using (MD5 md5Hash = MD5.Create())
            {
                StringBuilder sb = new StringBuilder();

                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(GetString()));
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
        public void MineBlock(int difficulty)
        {
            var sb = new StringBuilder();
            for(int i = 0; i < difficulty; i++){
                sb.Append("0");
            }

            string tmp = CalculateHash();
            while (!tmp.Substring(0, difficulty).Equals(sb.ToString()))
            {
                Nonce++;
                tmp = CalculateHash();
            }
            
            Date = DateTime.UtcNow;
            Console.WriteLine("*** Block mined.");
        }

        public string GetString()
        {
            var sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat(@" id:{0}, date:{1}, data:{2}, prevHash:{3}, nonce:{4} ", 
                            Id, Date, Data, PrevHash, Nonce );
            sb.Append("}");
            return sb.ToString();
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat(@" id:{0}, date:{1}, data:{2}, hash:{3} prevHash:{4}, nonce:{5} ", 
                            Id, Date, Data, Hash, PrevHash, Nonce );
            sb.Append("}");
            return sb.ToString();
        }
    }

    class BlockChain{
        public List<Block> Blocks = new List<Block>();
        private const int difficulty = 5;

        public BlockChain()
        {
            var prevHash = "000";
            var data = "{ balance:250 }";
            var genesisBlock = new Block { 
                Id = 0, Date = DateTime.UtcNow, Data = data, PrevHash = prevHash, Nonce = 0 
            };
            Blocks.Add(genesisBlock);

        }

        public Block GetLastBlock()
        {
            return Blocks[Blocks.Count - 1];
        }

        public bool IsValid(){
            for(var i = 1; i < Blocks.Count; i++)
            {
                var currentBlock = Blocks[i];
                var previousBlock = Blocks[i-1];
                if( currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }
                if(currentBlock.PrevHash != previousBlock.Hash)
                {
                    return false;
                }
            }

            return true;
        }

        public void AddBlock(Block block){
            block.PrevHash = GetLastBlock().Hash;
            block.MineBlock(difficulty);
            Blocks.Add(block);
        }

        public void Print(){
            foreach(var block in Blocks)
            {
                Console.WriteLine(block.ToString());
            }
        }
    }
}