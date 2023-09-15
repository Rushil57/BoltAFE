﻿namespace BoltAFE.Domain.AFE
{
    public interface IAFERepository
    {
        string GetTypes();
        string GetCategories();
        bool InsertComment(int afeHDRID, string message, int userID);
        string GetComments(int afeHDRID, int userID);
        int CreateFile(int afeHDRID, string docDiscription, string filePath);
        string GetDocs(int afeHDRID);
        bool DeleteDoc(int afeHDRID, int docID);
        string GetAllAFE();
        string GetAFE(int afeHDRID);
    }
}