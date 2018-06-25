namespace Model.Resources
{
    /// <summary>
    /// This enum consists of the keys contained in the Errors.resx
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
    public enum ErrorMessageKeys
    {
        AccessDenied,
        BillingQueryError,
        BundleNotFound,
        CriteriaNotFound,
        CustomFieldValueNotFound,
        DefaultErrorMessage,
        EmailNotProvided,
        EntitlementsNotFound,
        FreePaymentMethodNotFound,
        GroupsNotFound,
        InvalidAtsId,
        InvalidBillingCode,
        InvalidBundleId,
        InvalidBundleName,
        InvalidCustomFieldDisplayLabel,
        InvalidCustomFieldName,
        InvalidRequeueJobPostingStatus,
        InvalidChildMediaID,
        InvalidCustomFieldID,
        InvalidCustomFieldOptionText,
        InvalidCustomFieldOptionOrder,
        InvalidCustomFieldOptionValue,
        InvalidErrorTicketKey,
        InvalidGroupId,
        InvalidId,
        InvalidImportXmlContent,
        InvalidImportXmlTitle,
        InvalidLicenseId,
        InvalidMediaPaymentConfig,
        InvalidMediaId,
        InvalidMediaCode,
        InvalidMembershipId,
        InvalidMembershipType,
        InvalidOrganizationID,
        InvalidPaymentMethod,
        InvalidParentMediaID,
        InvalidReferenceNumber,
        InvalidRoleKey,
        InvalidSearchContent,
        InvalidSectionID,
        InvalidSelecion,
        InvalidTenantId,
        InvalidUserId,
        BoardNotFound,
        MappingNotFound,
        PostingCardinalityError,
        PostingTransferTypeError,
        SchedulerSynchronizationError,
        LicenseNotFound,
        LogNotSubmitted,
        MembershipNotFound,
        NoContent,
        NoGroupIdForJob,
        NoMediaAssociated,
        NoMembershipOrPricingFound,
        NoPaymentOptionsFound,
        NoSpecifiedJobBoardActionType,
        NotEntitledToEditJob,
        NotEntiledToEditPostedJob,
        NotEntitledToPostFree,
        NotEntitledToPostMembership,
        PaymentMethodNotFound,
        UnableToObtainLicenseId,
        UnableToSaveJobMediaMapping,
        UserNotFound,
        CustomFieldUniqueValue,
        NoTransactionInventory,
        ManualLicenseNotFound,
        NoManualTransactionInventory,
        NotEntitledToCreateStandaloneJob,
        ClientOrGroupsNotFound,
        ClientOrGroupsBeenSubmittedAlready
    }
}
