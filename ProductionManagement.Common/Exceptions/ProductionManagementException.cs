namespace ProductionManagement.Common.Exceptions
{
    public class ProductionManagementException : Exception
    {
        public string PMMessage { get; set; }

        public ProductionManagementException(string pmMessage)
        {
            PMMessage = pmMessage;
        }
    }
}