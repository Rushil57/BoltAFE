namespace BoltAFE.Domain.AFE
{
    public interface IAFERepository
    {
        string GetTypes();
        string GetTypesRecordDetails(int month, int year);
        string GetCategories();
        bool InsertComment(int afeHDRID, string message, int userID);
        string GetComments(int afeHDRID, int userID);
        int CreateFile(int afeHDRID, string docDiscription, string filePath);
        string GetDocs(int afeHDRID);
        bool DeleteDoc(int afeHDRID, int docID);
        string GetAllAFE();
        string GetAFE(int afeHDRID);
        string GetAFEHdrAprvlHistory(int afeHDRID);

        bool DeleteAFEHDR(int afeHDRID);

        bool CheckIfDuplicateAFENum(string afeHDR);
        bool SaveHDRAndDTL(string afeHDR,string afeHDRDTL,bool isApproveAFE, bool isDuplicateAFENum,bool isAppEditSave);

        bool ApproveAFE(int afeHDRID);
    }
}