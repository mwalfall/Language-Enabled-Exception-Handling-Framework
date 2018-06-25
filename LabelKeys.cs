namespace Model.Resources
{
    /// <summary>
    /// This enum consists of the keys contained in the Labels.resx
    /// 
    /// These keys are used by engineers when composing exceptions that inherit
    /// from the BusinessException class.
    /// 
    /// When a new entry is made to that file the key must be added to this file. 
    /// If not done the engineers will not have the key to choose from when
    /// designing custom exceptions.
    /// 
    /// ------ Maintain this list in alphabetic order. -------
    /// </summary>
    /// 
    public enum LabelKeys
    {
        Address,
        BillingCode,
        CandidateResponseUrl,
        City,
        CompanyName,
        ContactName,
        Country,
        GroupName,
        JobCategory,
        JobDescription,
        Language,
        NotesForInvoice,
        PoNumber,
        ReferenceNumber,
        ResumesEmail,
        SalaryHigh,
        SalaryLow,
        State,
        ZipCode
    }
}
