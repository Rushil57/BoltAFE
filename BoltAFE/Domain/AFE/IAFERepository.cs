using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoltAFE.Domain.AFE
{
    public interface IAFERepository
    {
        string GetTypes();
        string GetCategories();
        bool InsertComment(int afeHDRID, string message, int userID);
        int CreateFile(int afeHDRID, string docDiscription, string filePath);
        string GetDocs(int afeHDRID);
        string GetAllAFE();
    }
}