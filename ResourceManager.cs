namespace Model.Resources
{
    public static class ResourceManager
    {
        /// <summary>
        /// Obtain the string representation from an enum.
        /// </summary>
        /// <param name="resourceKey">The selected UserMessageErrorKeys object</param>
        /// 
        public static string GetResourceKeyString(ErrorMessageKeys resourceKey)
        {
            return resourceKey.ToString("g");
        }

        /// <summary>
        /// Obtain the string representation from an enum.
        /// </summary>
        /// <param name="resourceKey">The selected UserMessageErrorKeys object</param>
        /// 
        public static string GetResourceKeyString(LabelKeys resourceKey)
        {
            return resourceKey.ToString("g");
        }
    }
}