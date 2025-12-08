namespace SmartFactoryERP.Domain.Common
{
    /// <summary>
    /// ÃÓãÇÁ ÇáÕáÇÍíÇÊ ÇáËÇÈÊÉ İí ÇáäÙÇã
    /// </summary>
    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Employee = "Employee";
        public const string Viewer = "Viewer";
        
        // ÕáÇÍíÇÊ ãÊÎÕÕÉ ÍÓÈ ÇáãæÏíæá
        public const string InventoryManager = "InventoryManager";
        public const string ProductionManager = "ProductionManager";
        public const string HRManager = "HRManager";
        public const string PurchasingManager = "PurchasingManager";
        public const string SalesManager = "SalesManager";

        /// <summary>
        /// ÌãíÚ ÇáÕáÇÍíÇÊ ÇáãÊÇÍÉ
        /// </summary>
        public static readonly string[] AllRoles = 
        {
            SuperAdmin, Admin, Manager, Employee, Viewer,
            InventoryManager, ProductionManager, HRManager, 
            PurchasingManager, SalesManager
        };
    }
}