using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace InventoryManagement.Models
{
    public class GeneralModels
    {
        public byte[] ConvertToBytes(HttpPostedFileBase document)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(document.InputStream);
            imageBytes = reader.ReadBytes((int)document.ContentLength);
            return imageBytes;
        }
    }

    public class GeneralControl
    {
        //public List<WorkFlows> WorkFlows { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        //public string WorkFlowDesc { get; set; }
    }

    public class CustomControl3Val
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string Label { get; set; }
    }
}