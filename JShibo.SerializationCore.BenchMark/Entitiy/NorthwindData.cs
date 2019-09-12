﻿//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data.Linq;
//using System.Data.Linq.Mapping;
//using System.Linq;
//using System.Reflection;
//using System.Text;

//namespace JShibo.Serialization.BenchMark.Entitiy
//{

//    [System.Data.Linq.Mapping.DatabaseAttribute(Name = "NORTHWND")]
//    public partial class Northwind : System.Data.Linq.DataContext
//    {

//        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

//        #region Extensibility Method Definitions
//        partial void OnCreated();
//        partial void InsertActiveProductsFedarated(ActiveProductsFedarated instance);
//        partial void UpdateActiveProductsFedarated(ActiveProductsFedarated instance);
//        partial void DeleteActiveProductsFedarated(ActiveProductsFedarated instance);
//        partial void InsertAddressSplit(AddressSplit instance);
//        partial void UpdateAddressSplit(AddressSplit instance);
//        partial void DeleteAddressSplit(AddressSplit instance);
//        partial void InsertBaseContactSplit(BaseContactSplit instance);
//        partial void UpdateBaseContactSplit(BaseContactSplit instance);
//        partial void DeleteBaseContactSplit(BaseContactSplit instance);
//        partial void InsertBaseProductsFedarated(BaseProductsFedarated instance);
//        partial void UpdateBaseProductsFedarated(BaseProductsFedarated instance);
//        partial void DeleteBaseProductsFedarated(BaseProductsFedarated instance);
//        partial void InsertCategory(Category instance);
//        partial void UpdateCategory(Category instance);
//        partial void DeleteCategory(Category instance);
//        partial void InsertContactNameSplit(ContactNameSplit instance);
//        partial void UpdateContactNameSplit(ContactNameSplit instance);
//        partial void DeleteContactNameSplit(ContactNameSplit instance);
//        partial void InsertContact(Contact instance);
//        partial void UpdateContact(Contact instance);
//        partial void DeleteContact(Contact instance);
//        partial void InsertCustomerCustomerDemo(CustomerCustomerDemo instance);
//        partial void UpdateCustomerCustomerDemo(CustomerCustomerDemo instance);
//        partial void DeleteCustomerCustomerDemo(CustomerCustomerDemo instance);
//        partial void InsertCustomerDemographic(CustomerDemographic instance);
//        partial void UpdateCustomerDemographic(CustomerDemographic instance);
//        partial void DeleteCustomerDemographic(CustomerDemographic instance);
//        partial void InsertCustomer(Customer instance);
//        partial void UpdateCustomer(Customer instance);
//        partial void DeleteCustomer(Customer instance);
//        partial void InsertDiscontinuedProductsFedarated(DiscontinuedProductsFedarated instance);
//        partial void UpdateDiscontinuedProductsFedarated(DiscontinuedProductsFedarated instance);
//        partial void DeleteDiscontinuedProductsFedarated(DiscontinuedProductsFedarated instance);
//        partial void InsertEmployee(Employee instance);
//        partial void UpdateEmployee(Employee instance);
//        partial void DeleteEmployee(Employee instance);
//        partial void InsertEmployeeSplit(EmployeeSplit instance);
//        partial void UpdateEmployeeSplit(EmployeeSplit instance);
//        partial void DeleteEmployeeSplit(EmployeeSplit instance);
//        partial void InsertEmployeeTerritory(EmployeeTerritory instance);
//        partial void UpdateEmployeeTerritory(EmployeeTerritory instance);
//        partial void DeleteEmployeeTerritory(EmployeeTerritory instance);
//        partial void InsertOrderDetail(OrderDetail instance);
//        partial void UpdateOrderDetail(OrderDetail instance);
//        partial void DeleteOrderDetail(OrderDetail instance);
//        partial void InsertOrder(Order instance);
//        partial void UpdateOrder(Order instance);
//        partial void DeleteOrder(Order instance);
//        partial void InsertProduct(Product instance);
//        partial void UpdateProduct(Product instance);
//        partial void DeleteProduct(Product instance);
//        partial void InsertRegion(Region instance);
//        partial void UpdateRegion(Region instance);
//        partial void DeleteRegion(Region instance);
//        partial void InsertShipper(Shipper instance);
//        partial void UpdateShipper(Shipper instance);
//        partial void DeleteShipper(Shipper instance);
//        partial void InsertSupplier(Supplier instance);
//        partial void UpdateSupplier(Supplier instance);
//        partial void DeleteSupplier(Supplier instance);
//        partial void InsertTerritory(Territory instance);
//        partial void UpdateTerritory(Territory instance);
//        partial void DeleteTerritory(Territory instance);
//        #endregion

//        static Northwind()
//        {
//        }

//        public Northwind(string connection) :
//            base(connection, mappingSource)
//        {
//            OnCreated();
//        }

//        public Northwind(System.Data.IDbConnection connection) :
//            base(connection, mappingSource)
//        {
//            OnCreated();
//        }

//        public Northwind(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
//            base(connection, mappingSource)
//        {
//            OnCreated();
//        }

//        public Northwind(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
//            base(connection, mappingSource)
//        {
//            OnCreated();
//        }

//        public System.Data.Linq.Table<ActiveProductsFedarated> ActiveProductsFedarateds
//        {
//            get
//            {
//                return this.GetTable<ActiveProductsFedarated>();
//            }
//        }

//        public System.Data.Linq.Table<AddressSplit> AddressSplits
//        {
//            get
//            {
//                return this.GetTable<AddressSplit>();
//            }
//        }

//        public System.Data.Linq.Table<AlphabeticalListOfProduct> AlphabeticalListOfProducts
//        {
//            get
//            {
//                return this.GetTable<AlphabeticalListOfProduct>();
//            }
//        }

//        public System.Data.Linq.Table<BaseContactSplit> BaseContactSplits
//        {
//            get
//            {
//                return this.GetTable<BaseContactSplit>();
//            }
//        }

//        public System.Data.Linq.Table<BaseProductsFedarated> BaseProductsFedarateds
//        {
//            get
//            {
//                return this.GetTable<BaseProductsFedarated>();
//            }
//        }

//        public System.Data.Linq.Table<Category> Categories
//        {
//            get
//            {
//                return this.GetTable<Category>();
//            }
//        }

//        public System.Data.Linq.Table<CategorySalesFor1997> CategorySalesFor1997s
//        {
//            get
//            {
//                return this.GetTable<CategorySalesFor1997>();
//            }
//        }

//        public System.Data.Linq.Table<ContactNameSplit> ContactNameSplits
//        {
//            get
//            {
//                return this.GetTable<ContactNameSplit>();
//            }
//        }

//        public System.Data.Linq.Table<Contact> Contacts
//        {
//            get
//            {
//                return this.GetTable<Contact>();
//            }
//        }

//        public System.Data.Linq.Table<CurrentProductList> CurrentProductLists
//        {
//            get
//            {
//                return this.GetTable<CurrentProductList>();
//            }
//        }

//        public System.Data.Linq.Table<CustomerAndSuppliersByCity> CustomerAndSuppliersByCities
//        {
//            get
//            {
//                return this.GetTable<CustomerAndSuppliersByCity>();
//            }
//        }

//        public System.Data.Linq.Table<CustomerCustomerDemo> CustomerCustomerDemos
//        {
//            get
//            {
//                return this.GetTable<CustomerCustomerDemo>();
//            }
//        }

//        public System.Data.Linq.Table<CustomerDemographic> CustomerDemographics
//        {
//            get
//            {
//                return this.GetTable<CustomerDemographic>();
//            }
//        }

//        public System.Data.Linq.Table<Customer> Customers
//        {
//            get
//            {
//                return this.GetTable<Customer>();
//            }
//        }

//        public System.Data.Linq.Table<DiscontinuedProductsFedarated> DiscontinuedProductsFedarateds
//        {
//            get
//            {
//                return this.GetTable<DiscontinuedProductsFedarated>();
//            }
//        }

//        public System.Data.Linq.Table<Employee> Employees
//        {
//            get
//            {
//                return this.GetTable<Employee>();
//            }
//        }

//        public System.Data.Linq.Table<EmployeeSplit> EmployeeSplits
//        {
//            get
//            {
//                return this.GetTable<EmployeeSplit>();
//            }
//        }

//        public System.Data.Linq.Table<EmployeeTerritory> EmployeeTerritories
//        {
//            get
//            {
//                return this.GetTable<EmployeeTerritory>();
//            }
//        }

//        public System.Data.Linq.Table<Invoices> Invoices
//        {
//            get
//            {
//                return this.GetTable<Invoices>();
//            }
//        }

//        public System.Data.Linq.Table<OrderDetail> OrderDetails
//        {
//            get
//            {
//                return this.GetTable<OrderDetail>();
//            }
//        }

//        public System.Data.Linq.Table<OrderDetailsExtended> OrderDetailsExtendeds
//        {
//            get
//            {
//                return this.GetTable<OrderDetailsExtended>();
//            }
//        }

//        public System.Data.Linq.Table<OrderSubtotal> OrderSubtotals
//        {
//            get
//            {
//                return this.GetTable<OrderSubtotal>();
//            }
//        }

//        public System.Data.Linq.Table<Order> Orders
//        {
//            get
//            {
//                return this.GetTable<Order>();
//            }
//        }

//        public System.Data.Linq.Table<OrdersQry> OrdersQries
//        {
//            get
//            {
//                return this.GetTable<OrdersQry>();
//            }
//        }

//        public System.Data.Linq.Table<ProductSalesFor1997> ProductSalesFor1997s
//        {
//            get
//            {
//                return this.GetTable<ProductSalesFor1997>();
//            }
//        }

//        public System.Data.Linq.Table<Product> Products
//        {
//            get
//            {
//                return this.GetTable<Product>();
//            }
//        }

//        public System.Data.Linq.Table<ProductsAboveAveragePrice> ProductsAboveAveragePrices
//        {
//            get
//            {
//                return this.GetTable<ProductsAboveAveragePrice>();
//            }
//        }

//        public System.Data.Linq.Table<ProductsByCategory> ProductsByCategories
//        {
//            get
//            {
//                return this.GetTable<ProductsByCategory>();
//            }
//        }

//        public System.Data.Linq.Table<QuarterlyOrder> QuarterlyOrders
//        {
//            get
//            {
//                return this.GetTable<QuarterlyOrder>();
//            }
//        }

//        public System.Data.Linq.Table<Region> Regions
//        {
//            get
//            {
//                return this.GetTable<Region>();
//            }
//        }

//        public System.Data.Linq.Table<SalesByCategory> SalesByCategories
//        {
//            get
//            {
//                return this.GetTable<SalesByCategory>();
//            }
//        }

//        public System.Data.Linq.Table<SalesTotalsByAmount> SalesTotalsByAmounts
//        {
//            get
//            {
//                return this.GetTable<SalesTotalsByAmount>();
//            }
//        }

//        public System.Data.Linq.Table<Shipper> Shippers
//        {
//            get
//            {
//                return this.GetTable<Shipper>();
//            }
//        }

//        public System.Data.Linq.Table<SummaryOfSalesByQuarter> SummaryOfSalesByQuarters
//        {
//            get
//            {
//                return this.GetTable<SummaryOfSalesByQuarter>();
//            }
//        }

//        public System.Data.Linq.Table<SummaryOfSalesByYear> SummaryOfSalesByYears
//        {
//            get
//            {
//                return this.GetTable<SummaryOfSalesByYear>();
//            }
//        }

//        public System.Data.Linq.Table<Supplier> Suppliers
//        {
//            get
//            {
//                return this.GetTable<Supplier>();
//            }
//        }

//        public System.Data.Linq.Table<Territory> Territories
//        {
//            get
//            {
//                return this.GetTable<Territory>();
//            }
//        }

//        [Function(Name = "dbo.Customers By City")]
//        public ISingleResult<CustomersByCityResult> CustomersByCity([Parameter(DbType = "NVarChar(20)")] string param1)
//        {
//            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), param1);
//            return ((ISingleResult<CustomersByCityResult>)(result.ReturnValue));
//        }

//        [Function(Name = "dbo.Customers Count By Region")]
//        [return: Parameter(DbType = "Int")]
//        public int CustomersCountByRegion([Parameter(DbType = "NVarChar(15)")] string param1)
//        {
//            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), param1);
//            return ((int)(result.ReturnValue));
//        }

//        [Function(Name = "dbo.CustOrderHist")]
//        public ISingleResult<CustOrderHistResult> CustomerOrderHistory([Parameter(Name = "CustomerID", DbType = "NChar(5)")] string customerID)
//        {
//            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), customerID);
//            return ((ISingleResult<CustOrderHistResult>)(result.ReturnValue));
//        }

//        [Function(Name = "dbo.CustOrdersDetail")]
//        public ISingleResult<CustOrdersDetailResult> CustOrdersDetail([Parameter(Name = "OrderID", DbType = "Int")] System.Nullable<int> orderID)
//        {
//            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), orderID);
//            return ((ISingleResult<CustOrdersDetailResult>)(result.ReturnValue));
//        }

//        [Function(Name = "dbo.CustOrdersOrders")]
//        public ISingleResult<CustOrdersOrdersResult> CustOrdersOrders([Parameter(Name = "CustomerID", DbType = "NChar(5)")] string customerID)
//        {
//            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), customerID);
//            return ((ISingleResult<CustOrdersOrdersResult>)(result.ReturnValue));
//        }

//        [Function(Name = "dbo.CustOrderTotal")]
//        [return: Parameter(DbType = "Int")]
//        public int CustomerTotalSales([Parameter(Name = "CustomerID", DbType = "NChar(5)")] string customerID, [Parameter(Name = "TotalSales", DbType = "Money")] ref System.Nullable<decimal> totalSales)
//        {
//            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), customerID, totalSales);
//            totalSales = ((System.Nullable<decimal>)(result.GetParameterValue(1)));
//            return ((int)(result.ReturnValue));
//        }

//        [Function(Name = "dbo.Employee Sales by Country")]
//        public ISingleResult<EmployeeSalesByCountryResult> EmployeeSalesByCountry([Parameter(Name = "Beginning_Date", DbType = "DateTime")] System.Nullable<System.DateTime> beginning_Date, [Parameter(Name = "Ending_Date", DbType = "DateTime")] System.Nullable<System.DateTime> ending_Date)
//        {
//            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), beginning_Date, ending_Date);
//            return ((ISingleResult<EmployeeSalesByCountryResult>)(result.ReturnValue));
//        }

//        [Function(Name = "dbo.Get Customer And Orders")]
//        [ResultType(typeof(CustomerResultSet))]
//        [ResultType(typeof(OrdersResultSet))]
//        public IMultipleResults GetCustomerAndOrders([Parameter(Name = "CustomerID", DbType = "NChar(5)")] string customerID)
//        {
//            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), customerID);
//            return ((IMultipleResults)(result.ReturnValue));
//        }

//        [Function(Name = "dbo.MinUnitPriceByCategory", IsComposable = true)]
//        [return: Parameter(DbType = "Money")]
//        public System.Nullable<decimal> MinUnitPriceByCategory([Parameter(DbType = "Int")] System.Nullable<int> categoryID)
//        {
//            return ((System.Nullable<decimal>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), categoryID).ReturnValue));
//        }

//        [Function(Name = "dbo.ProductsUnderThisUnitPrice", IsComposable = true)]
//        public IQueryable<ProductsUnderThisUnitPriceResult> ProductsUnderThisUnitPrice([Parameter(DbType = "Money")] System.Nullable<decimal> price)
//        {
//            return this.CreateMethodCallQuery<ProductsUnderThisUnitPriceResult>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), price);
//        }

//        [Function(Name = "dbo.Sales by Year")]
//        public ISingleResult<SalesByYearResult> SalesByYear([Parameter(Name = "Beginning_Date", DbType = "DateTime")] System.Nullable<System.DateTime> beginning_Date, [Parameter(Name = "Ending_Date", DbType = "DateTime")] System.Nullable<System.DateTime> ending_Date)
//        {
//            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), beginning_Date, ending_Date);
//            return ((ISingleResult<SalesByYearResult>)(result.ReturnValue));
//        }

//        [Function(Name = "dbo.SalesByCategory")]
//        public ISingleResult<SalesByCategoryResult> SalesByCategory([Parameter(Name = "CategoryName", DbType = "NVarChar(15)")] string categoryName, [Parameter(Name = "OrdYear", DbType = "NVarChar(4)")] string ordYear)
//        {
//            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), categoryName, ordYear);
//            return ((ISingleResult<SalesByCategoryResult>)(result.ReturnValue));
//        }

//        [Function(Name = "dbo.Ten Most Expensive Products")]
//        public ISingleResult<TenMostExpensiveProductsResult> TenMostExpensiveProducts()
//        {
//            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
//            return ((ISingleResult<TenMostExpensiveProductsResult>)(result.ReturnValue));
//        }

//        [Function(Name = "dbo.TotalProductUnitPriceByCategory", IsComposable = true)]
//        [return: Parameter(DbType = "Money")]
//        public System.Nullable<decimal> TotalProductUnitPriceByCategory([Parameter(DbType = "Int")] System.Nullable<int> categoryID)
//        {
//            return ((System.Nullable<decimal>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), categoryID).ReturnValue));
//        }

//        [Function(Name = "dbo.Whole Or Partial Customers Set")]
//        [ResultType(typeof(WholeCustomersSetResult))]
//        [ResultType(typeof(PartialCustomersSetResult))]
//        public IMultipleResults WholeOrPartialCustomersSet([Parameter(DbType = "Int")] System.Nullable<int> param1)
//        {
//            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), param1);
//            return ((IMultipleResults)(result.ReturnValue));
//        }
//    }

//    [Table(Name = "dbo.ActiveProductsFedarated")]
//    public partial class ActiveProductsFedarated : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _ProductID;

//        private string _ProductName;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnProductIDChanging(int value);
//        partial void OnProductIDChanged();
//        partial void OnProductNameChanging(string value);
//        partial void OnProductNameChanged();
//        #endregion

//        public ActiveProductsFedarated()
//        {
//            OnCreated();
//        }

//        [Column(Storage = "_ProductID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
//        public int ProductID
//        {
//            get
//            {
//                return this._ProductID;
//            }
//            set
//            {
//                if ((this._ProductID != value))
//                {
//                    this.OnProductIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._ProductID = value;
//                    this.SendPropertyChanged("ProductID");
//                    this.OnProductIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this.OnProductNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._ProductName = value;
//                    this.SendPropertyChanged("ProductName");
//                    this.OnProductNameChanged();
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }

//    [Table(Name = "dbo.AddressSplit")]
//    public partial class AddressSplit : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _ID;

//        private string _Address;

//        private string _City;

//        private string _Region;

//        private string _PostalCode;

//        private string _Country;

//        private string _ContactType;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnIDChanging(int value);
//        partial void OnIDChanged();
//        partial void OnAddressChanging(string value);
//        partial void OnAddressChanged();
//        partial void OnCityChanging(string value);
//        partial void OnCityChanged();
//        partial void OnRegionChanging(string value);
//        partial void OnRegionChanged();
//        partial void OnPostalCodeChanging(string value);
//        partial void OnPostalCodeChanged();
//        partial void OnCountryChanging(string value);
//        partial void OnCountryChanged();
//        partial void OnContactTypeChanging(string value);
//        partial void OnContactTypeChanged();
//        #endregion

//        public AddressSplit()
//        {
//            OnCreated();
//        }

//        [Column(Storage = "_ID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
//        public int ID
//        {
//            get
//            {
//                return this._ID;
//            }
//            set
//            {
//                if ((this._ID != value))
//                {
//                    this.OnIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._ID = value;
//                    this.SendPropertyChanged("ID");
//                    this.OnIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Address", DbType = "NVarChar(60)")]
//        public string Address
//        {
//            get
//            {
//                return this._Address;
//            }
//            set
//            {
//                if ((this._Address != value))
//                {
//                    this.OnAddressChanging(value);
//                    this.SendPropertyChanging();
//                    this._Address = value;
//                    this.SendPropertyChanged("Address");
//                    this.OnAddressChanged();
//                }
//            }
//        }

//        [Column(Storage = "_City", DbType = "NVarChar(15)")]
//        public string City
//        {
//            get
//            {
//                return this._City;
//            }
//            set
//            {
//                if ((this._City != value))
//                {
//                    this.OnCityChanging(value);
//                    this.SendPropertyChanging();
//                    this._City = value;
//                    this.SendPropertyChanged("City");
//                    this.OnCityChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Region", DbType = "NVarChar(15)")]
//        public string Region
//        {
//            get
//            {
//                return this._Region;
//            }
//            set
//            {
//                if ((this._Region != value))
//                {
//                    this.OnRegionChanging(value);
//                    this.SendPropertyChanging();
//                    this._Region = value;
//                    this.SendPropertyChanged("Region");
//                    this.OnRegionChanged();
//                }
//            }
//        }

//        [Column(Storage = "_PostalCode", DbType = "NVarChar(50)")]
//        public string PostalCode
//        {
//            get
//            {
//                return this._PostalCode;
//            }
//            set
//            {
//                if ((this._PostalCode != value))
//                {
//                    this.OnPostalCodeChanging(value);
//                    this.SendPropertyChanging();
//                    this._PostalCode = value;
//                    this.SendPropertyChanged("PostalCode");
//                    this.OnPostalCodeChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Country", DbType = "NVarChar(15)")]
//        public string Country
//        {
//            get
//            {
//                return this._Country;
//            }
//            set
//            {
//                if ((this._Country != value))
//                {
//                    this.OnCountryChanging(value);
//                    this.SendPropertyChanging();
//                    this._Country = value;
//                    this.SendPropertyChanged("Country");
//                    this.OnCountryChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ContactType", DbType = "NVarChar(50)")]
//        public string ContactType
//        {
//            get
//            {
//                return this._ContactType;
//            }
//            set
//            {
//                if ((this._ContactType != value))
//                {
//                    this.OnContactTypeChanging(value);
//                    this.SendPropertyChanging();
//                    this._ContactType = value;
//                    this.SendPropertyChanged("ContactType");
//                    this.OnContactTypeChanged();
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }

//    [Table(Name = "dbo.Alphabetical list of products")]
//    public partial class AlphabeticalListOfProduct
//    {

//        private int _ProductID;

//        private string _ProductName;

//        private System.Nullable<int> _SupplierID;

//        private System.Nullable<int> _CategoryID;

//        private string _QuantityPerUnit;

//        private System.Nullable<decimal> _UnitPrice;

//        private System.Nullable<short> _UnitsInStock;

//        private System.Nullable<short> _UnitsOnOrder;

//        private System.Nullable<short> _ReorderLevel;

//        private bool _Discontinued;

//        private string _CategoryName;

//        public AlphabeticalListOfProduct()
//        {
//        }

//        [Column(Storage = "_ProductID", DbType = "Int NOT NULL IDENTITY")]
//        public int ProductID
//        {
//            get
//            {
//                return this._ProductID;
//            }
//            set
//            {
//                if ((this._ProductID != value))
//                {
//                    this._ProductID = value;
//                }
//            }
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this._ProductName = value;
//                }
//            }
//        }

//        [Column(Storage = "_SupplierID", DbType = "Int")]
//        public System.Nullable<int> SupplierID
//        {
//            get
//            {
//                return this._SupplierID;
//            }
//            set
//            {
//                if ((this._SupplierID != value))
//                {
//                    this._SupplierID = value;
//                }
//            }
//        }

//        [Column(Storage = "_CategoryID", DbType = "Int")]
//        public System.Nullable<int> CategoryID
//        {
//            get
//            {
//                return this._CategoryID;
//            }
//            set
//            {
//                if ((this._CategoryID != value))
//                {
//                    this._CategoryID = value;
//                }
//            }
//        }

//        [Column(Storage = "_QuantityPerUnit", DbType = "NVarChar(20)")]
//        public string QuantityPerUnit
//        {
//            get
//            {
//                return this._QuantityPerUnit;
//            }
//            set
//            {
//                if ((this._QuantityPerUnit != value))
//                {
//                    this._QuantityPerUnit = value;
//                }
//            }
//        }

//        [Column(Storage = "_UnitPrice", DbType = "Money")]
//        public System.Nullable<decimal> UnitPrice
//        {
//            get
//            {
//                return this._UnitPrice;
//            }
//            set
//            {
//                if ((this._UnitPrice != value))
//                {
//                    this._UnitPrice = value;
//                }
//            }
//        }

//        [Column(Storage = "_UnitsInStock", DbType = "SmallInt")]
//        public System.Nullable<short> UnitsInStock
//        {
//            get
//            {
//                return this._UnitsInStock;
//            }
//            set
//            {
//                if ((this._UnitsInStock != value))
//                {
//                    this._UnitsInStock = value;
//                }
//            }
//        }

//        [Column(Storage = "_UnitsOnOrder", DbType = "SmallInt")]
//        public System.Nullable<short> UnitsOnOrder
//        {
//            get
//            {
//                return this._UnitsOnOrder;
//            }
//            set
//            {
//                if ((this._UnitsOnOrder != value))
//                {
//                    this._UnitsOnOrder = value;
//                }
//            }
//        }

//        [Column(Storage = "_ReorderLevel", DbType = "SmallInt")]
//        public System.Nullable<short> ReorderLevel
//        {
//            get
//            {
//                return this._ReorderLevel;
//            }
//            set
//            {
//                if ((this._ReorderLevel != value))
//                {
//                    this._ReorderLevel = value;
//                }
//            }
//        }

//        [Column(Storage = "_Discontinued", DbType = "Bit NOT NULL")]
//        public bool Discontinued
//        {
//            get
//            {
//                return this._Discontinued;
//            }
//            set
//            {
//                if ((this._Discontinued != value))
//                {
//                    this._Discontinued = value;
//                }
//            }
//        }

//        [Column(Storage = "_CategoryName", DbType = "NVarChar(15) NOT NULL", CanBeNull = false)]
//        public string CategoryName
//        {
//            get
//            {
//                return this._CategoryName;
//            }
//            set
//            {
//                if ((this._CategoryName != value))
//                {
//                    this._CategoryName = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.BaseContactSplit")]
//    public partial class BaseContactSplit : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _ID;

//        private string _CompanyName;

//        private string _ContactName;

//        private string _Phone;

//        private string _ContactType;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnIDChanging(int value);
//        partial void OnIDChanged();
//        partial void OnCompanyNameChanging(string value);
//        partial void OnCompanyNameChanged();
//        partial void OnContactNameChanging(string value);
//        partial void OnContactNameChanged();
//        partial void OnPhoneChanging(string value);
//        partial void OnPhoneChanged();
//        partial void OnContactTypeChanging(string value);
//        partial void OnContactTypeChanged();
//        #endregion

//        public BaseContactSplit()
//        {
//            OnCreated();
//        }

//        [Column(Storage = "_ID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
//        public int ID
//        {
//            get
//            {
//                return this._ID;
//            }
//            set
//            {
//                if ((this._ID != value))
//                {
//                    this.OnIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._ID = value;
//                    this.SendPropertyChanged("ID");
//                    this.OnIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this.OnCompanyNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._CompanyName = value;
//                    this.SendPropertyChanged("CompanyName");
//                    this.OnCompanyNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ContactName", DbType = "NVarChar(30)")]
//        public string ContactName
//        {
//            get
//            {
//                return this._ContactName;
//            }
//            set
//            {
//                if ((this._ContactName != value))
//                {
//                    this.OnContactNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._ContactName = value;
//                    this.SendPropertyChanged("ContactName");
//                    this.OnContactNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Phone", DbType = "NVarChar(24)")]
//        public string Phone
//        {
//            get
//            {
//                return this._Phone;
//            }
//            set
//            {
//                if ((this._Phone != value))
//                {
//                    this.OnPhoneChanging(value);
//                    this.SendPropertyChanging();
//                    this._Phone = value;
//                    this.SendPropertyChanged("Phone");
//                    this.OnPhoneChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ContactType", DbType = "NVarChar(50)")]
//        public string ContactType
//        {
//            get
//            {
//                return this._ContactType;
//            }
//            set
//            {
//                if ((this._ContactType != value))
//                {
//                    this.OnContactTypeChanging(value);
//                    this.SendPropertyChanging();
//                    this._ContactType = value;
//                    this.SendPropertyChanged("ContactType");
//                    this.OnContactTypeChanged();
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }

//    [Table(Name = "dbo.BaseProductsFedarated")]
//    public partial class BaseProductsFedarated : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _ProductID;

//        private string _ProductName;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnProductIDChanging(int value);
//        partial void OnProductIDChanged();
//        partial void OnProductNameChanging(string value);
//        partial void OnProductNameChanged();
//        #endregion

//        public BaseProductsFedarated()
//        {
//            OnCreated();
//        }

//        [Column(Storage = "_ProductID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
//        public int ProductID
//        {
//            get
//            {
//                return this._ProductID;
//            }
//            set
//            {
//                if ((this._ProductID != value))
//                {
//                    this.OnProductIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._ProductID = value;
//                    this.SendPropertyChanged("ProductID");
//                    this.OnProductIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this.OnProductNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._ProductName = value;
//                    this.SendPropertyChanged("ProductName");
//                    this.OnProductNameChanged();
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }

//    [Table(Name = "dbo.Categories")]
//    public partial class Category : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _CategoryID;

//        private string _CategoryName;

//        private string _Description;

//        private System.Data.Linq.Binary _Picture;

//        private EntitySet<Product> _Products;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnCategoryIDChanging(int value);
//        partial void OnCategoryIDChanged();
//        partial void OnCategoryNameChanging(string value);
//        partial void OnCategoryNameChanged();
//        partial void OnDescriptionChanging(string value);
//        partial void OnDescriptionChanged();
//        partial void OnPictureChanging(System.Data.Linq.Binary value);
//        partial void OnPictureChanged();
//        #endregion

//        public Category()
//        {
//            this._Products = new EntitySet<Product>(new Action<Product>(this.attach_Products), new Action<Product>(this.detach_Products));
//            OnCreated();
//        }

//        [Column(Storage = "_CategoryID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
//        public int CategoryID
//        {
//            get
//            {
//                return this._CategoryID;
//            }
//            set
//            {
//                if ((this._CategoryID != value))
//                {
//                    this.OnCategoryIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._CategoryID = value;
//                    this.SendPropertyChanged("CategoryID");
//                    this.OnCategoryIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_CategoryName", DbType = "NVarChar(15) NOT NULL", CanBeNull = false)]
//        public string CategoryName
//        {
//            get
//            {
//                return this._CategoryName;
//            }
//            set
//            {
//                if ((this._CategoryName != value))
//                {
//                    this.OnCategoryNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._CategoryName = value;
//                    this.SendPropertyChanged("CategoryName");
//                    this.OnCategoryNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Description", DbType = "NText", UpdateCheck = UpdateCheck.Never)]
//        public string Description
//        {
//            get
//            {
//                return this._Description;
//            }
//            set
//            {
//                if ((this._Description != value))
//                {
//                    this.OnDescriptionChanging(value);
//                    this.SendPropertyChanging();
//                    this._Description = value;
//                    this.SendPropertyChanged("Description");
//                    this.OnDescriptionChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Picture", DbType = "Image", CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
//        public System.Data.Linq.Binary Picture
//        {
//            get
//            {
//                return this._Picture;
//            }
//            set
//            {
//                if ((this._Picture != value))
//                {
//                    this.OnPictureChanging(value);
//                    this.SendPropertyChanging();
//                    this._Picture = value;
//                    this.SendPropertyChanged("Picture");
//                    this.OnPictureChanged();
//                }
//            }
//        }

//        [Association(Name = "Category_Product", Storage = "_Products", OtherKey = "CategoryID")]
//        public EntitySet<Product> Products
//        {
//            get
//            {
//                return this._Products;
//            }
//            set
//            {
//                this._Products.Assign(value);
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        private void attach_Products(Product entity)
//        {
//            this.SendPropertyChanging();
//            entity.Category = this;
//            this.SendPropertyChanged("Products");
//        }

//        private void detach_Products(Product entity)
//        {
//            this.SendPropertyChanging();
//            entity.Category = null;
//            this.SendPropertyChanged("Products");
//        }
//    }

//    [Table(Name = "dbo.Category Sales for 1997")]
//    public partial class CategorySalesFor1997
//    {

//        private string _CategoryName;

//        private System.Nullable<decimal> _CategorySales;

//        public CategorySalesFor1997()
//        {
//        }

//        [Column(Storage = "_CategoryName", DbType = "NVarChar(15) NOT NULL", CanBeNull = false)]
//        public string CategoryName
//        {
//            get
//            {
//                return this._CategoryName;
//            }
//            set
//            {
//                if ((this._CategoryName != value))
//                {
//                    this._CategoryName = value;
//                }
//            }
//        }

//        [Column(Storage = "_CategorySales", DbType = "Money")]
//        public System.Nullable<decimal> CategorySales
//        {
//            get
//            {
//                return this._CategorySales;
//            }
//            set
//            {
//                if ((this._CategorySales != value))
//                {
//                    this._CategorySales = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.ContactNameSplit")]
//    public partial class ContactNameSplit : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _ID;

//        private string _Name;

//        private string _Title;

//        private string _Fax;

//        private string _ContactType;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnIDChanging(int value);
//        partial void OnIDChanged();
//        partial void OnNameChanging(string value);
//        partial void OnNameChanged();
//        partial void OnTitleChanging(string value);
//        partial void OnTitleChanged();
//        partial void OnFaxChanging(string value);
//        partial void OnFaxChanged();
//        partial void OnContactTypeChanging(string value);
//        partial void OnContactTypeChanged();
//        #endregion

//        public ContactNameSplit()
//        {
//            OnCreated();
//        }

//        [Column(Storage = "_ID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
//        public int ID
//        {
//            get
//            {
//                return this._ID;
//            }
//            set
//            {
//                if ((this._ID != value))
//                {
//                    this.OnIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._ID = value;
//                    this.SendPropertyChanged("ID");
//                    this.OnIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Name", DbType = "NVarChar(30)")]
//        public string Name
//        {
//            get
//            {
//                return this._Name;
//            }
//            set
//            {
//                if ((this._Name != value))
//                {
//                    this.OnNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._Name = value;
//                    this.SendPropertyChanged("Name");
//                    this.OnNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Title", DbType = "NVarChar(30)")]
//        public string Title
//        {
//            get
//            {
//                return this._Title;
//            }
//            set
//            {
//                if ((this._Title != value))
//                {
//                    this.OnTitleChanging(value);
//                    this.SendPropertyChanging();
//                    this._Title = value;
//                    this.SendPropertyChanged("Title");
//                    this.OnTitleChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Fax", DbType = "NVarChar(24)")]
//        public string Fax
//        {
//            get
//            {
//                return this._Fax;
//            }
//            set
//            {
//                if ((this._Fax != value))
//                {
//                    this.OnFaxChanging(value);
//                    this.SendPropertyChanging();
//                    this._Fax = value;
//                    this.SendPropertyChanged("Fax");
//                    this.OnFaxChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ContactType", DbType = "NVarChar(50)")]
//        public string ContactType
//        {
//            get
//            {
//                return this._ContactType;
//            }
//            set
//            {
//                if ((this._ContactType != value))
//                {
//                    this.OnContactTypeChanging(value);
//                    this.SendPropertyChanging();
//                    this._ContactType = value;
//                    this.SendPropertyChanged("ContactType");
//                    this.OnContactTypeChanged();
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }

//    [Table(Name = "dbo.Contacts")]
//    [InheritanceMapping(Code = "Unknown", Type = typeof(Contact), IsDefault = true)]
//    [InheritanceMapping(Code = "Employee", Type = typeof(EmployeeContact))]
//    [InheritanceMapping(Code = "Supplier", Type = typeof(SupplierContact))]
//    [InheritanceMapping(Code = "Customer", Type = typeof(CustomerContact))]
//    [InheritanceMapping(Code = "Shipper", Type = typeof(ShipperContact))]
//    public partial class Contact : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _ContactID;

//        private string _ContactType;

//        private string _CompanyName;

//        private string _Phone;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnContactIDChanging(int value);
//        partial void OnContactIDChanged();
//        partial void OnContactTypeChanging(string value);
//        partial void OnContactTypeChanged();
//        partial void OnCompanyNameChanging(string value);
//        partial void OnCompanyNameChanged();
//        partial void OnPhoneChanging(string value);
//        partial void OnPhoneChanged();
//        #endregion

//        public Contact()
//        {
//            OnCreated();
//        }

//        [Column(Storage = "_ContactID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
//        public int ContactID
//        {
//            get
//            {
//                return this._ContactID;
//            }
//            set
//            {
//                if ((this._ContactID != value))
//                {
//                    this.OnContactIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._ContactID = value;
//                    this.SendPropertyChanged("ContactID");
//                    this.OnContactIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ContactType", DbType = "NVarChar(50)", IsDiscriminator = true)]
//        public string ContactType
//        {
//            get
//            {
//                return this._ContactType;
//            }
//            set
//            {
//                if ((this._ContactType != value))
//                {
//                    this.OnContactTypeChanging(value);
//                    this.SendPropertyChanging();
//                    this._ContactType = value;
//                    this.SendPropertyChanged("ContactType");
//                    this.OnContactTypeChanged();
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this.OnCompanyNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._CompanyName = value;
//                    this.SendPropertyChanged("CompanyName");
//                    this.OnCompanyNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Phone", DbType = "NVarChar(24)")]
//        public string Phone
//        {
//            get
//            {
//                return this._Phone;
//            }
//            set
//            {
//                if ((this._Phone != value))
//                {
//                    this.OnPhoneChanging(value);
//                    this.SendPropertyChanging();
//                    this._Phone = value;
//                    this.SendPropertyChanged("Phone");
//                    this.OnPhoneChanged();
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }

//    public abstract partial class FullContact : Contact
//    {

//        private string _ContactName;

//        private string _ContactTitle;

//        private string _Address;

//        private string _City;

//        private string _Region;

//        private string _PostalCode;

//        private string _Country;

//        private string _Fax;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnContactNameChanging(string value);
//        partial void OnContactNameChanged();
//        partial void OnContactTitleChanging(string value);
//        partial void OnContactTitleChanged();
//        partial void OnAddressChanging(string value);
//        partial void OnAddressChanged();
//        partial void OnCityChanging(string value);
//        partial void OnCityChanged();
//        partial void OnRegionChanging(string value);
//        partial void OnRegionChanged();
//        partial void OnPostalCodeChanging(string value);
//        partial void OnPostalCodeChanged();
//        partial void OnCountryChanging(string value);
//        partial void OnCountryChanged();
//        partial void OnFaxChanging(string value);
//        partial void OnFaxChanged();
//        #endregion

//        public FullContact()
//        {
//            OnCreated();
//        }

//        [Column(Storage = "_ContactName", DbType = "NVarChar(30)")]
//        public string ContactName
//        {
//            get
//            {
//                return this._ContactName;
//            }
//            set
//            {
//                if ((this._ContactName != value))
//                {
//                    this.OnContactNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._ContactName = value;
//                    this.SendPropertyChanged("ContactName");
//                    this.OnContactNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ContactTitle", DbType = "NVarChar(30)")]
//        public string ContactTitle
//        {
//            get
//            {
//                return this._ContactTitle;
//            }
//            set
//            {
//                if ((this._ContactTitle != value))
//                {
//                    this.OnContactTitleChanging(value);
//                    this.SendPropertyChanging();
//                    this._ContactTitle = value;
//                    this.SendPropertyChanged("ContactTitle");
//                    this.OnContactTitleChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Address", DbType = "NVarChar(60)")]
//        public string Address
//        {
//            get
//            {
//                return this._Address;
//            }
//            set
//            {
//                if ((this._Address != value))
//                {
//                    this.OnAddressChanging(value);
//                    this.SendPropertyChanging();
//                    this._Address = value;
//                    this.SendPropertyChanged("Address");
//                    this.OnAddressChanged();
//                }
//            }
//        }

//        [Column(Storage = "_City", DbType = "NVarChar(15)")]
//        public string City
//        {
//            get
//            {
//                return this._City;
//            }
//            set
//            {
//                if ((this._City != value))
//                {
//                    this.OnCityChanging(value);
//                    this.SendPropertyChanging();
//                    this._City = value;
//                    this.SendPropertyChanged("City");
//                    this.OnCityChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Region", DbType = "NVarChar(15)")]
//        public string Region
//        {
//            get
//            {
//                return this._Region;
//            }
//            set
//            {
//                if ((this._Region != value))
//                {
//                    this.OnRegionChanging(value);
//                    this.SendPropertyChanging();
//                    this._Region = value;
//                    this.SendPropertyChanged("Region");
//                    this.OnRegionChanged();
//                }
//            }
//        }

//        [Column(Storage = "_PostalCode", DbType = "NVarChar(10)")]
//        public string PostalCode
//        {
//            get
//            {
//                return this._PostalCode;
//            }
//            set
//            {
//                if ((this._PostalCode != value))
//                {
//                    this.OnPostalCodeChanging(value);
//                    this.SendPropertyChanging();
//                    this._PostalCode = value;
//                    this.SendPropertyChanged("PostalCode");
//                    this.OnPostalCodeChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Country", DbType = "NVarChar(15)")]
//        public string Country
//        {
//            get
//            {
//                return this._Country;
//            }
//            set
//            {
//                if ((this._Country != value))
//                {
//                    this.OnCountryChanging(value);
//                    this.SendPropertyChanging();
//                    this._Country = value;
//                    this.SendPropertyChanged("Country");
//                    this.OnCountryChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Fax", DbType = "NVarChar(24)")]
//        public string Fax
//        {
//            get
//            {
//                return this._Fax;
//            }
//            set
//            {
//                if ((this._Fax != value))
//                {
//                    this.OnFaxChanging(value);
//                    this.SendPropertyChanging();
//                    this._Fax = value;
//                    this.SendPropertyChanged("Fax");
//                    this.OnFaxChanged();
//                }
//            }
//        }
//    }

//    public partial class EmployeeContact : FullContact
//    {

//        private string _PhotoPath;

//        private System.Data.Linq.Binary _Photo;

//        private string _Extension;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnPhotoPathChanging(string value);
//        partial void OnPhotoPathChanged();
//        partial void OnPhotoChanging(System.Data.Linq.Binary value);
//        partial void OnPhotoChanged();
//        partial void OnExtensionChanging(string value);
//        partial void OnExtensionChanged();
//        #endregion

//        public EmployeeContact()
//        {
//            OnCreated();
//        }

//        [Column(Storage = "_PhotoPath", DbType = "NVarChar(255)")]
//        public string PhotoPath
//        {
//            get
//            {
//                return this._PhotoPath;
//            }
//            set
//            {
//                if ((this._PhotoPath != value))
//                {
//                    this.OnPhotoPathChanging(value);
//                    this.SendPropertyChanging();
//                    this._PhotoPath = value;
//                    this.SendPropertyChanged("PhotoPath");
//                    this.OnPhotoPathChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Photo", DbType = "Image", CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
//        public System.Data.Linq.Binary Photo
//        {
//            get
//            {
//                return this._Photo;
//            }
//            set
//            {
//                if ((this._Photo != value))
//                {
//                    this.OnPhotoChanging(value);
//                    this.SendPropertyChanging();
//                    this._Photo = value;
//                    this.SendPropertyChanged("Photo");
//                    this.OnPhotoChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Extension", DbType = "NVarChar(4)")]
//        public string Extension
//        {
//            get
//            {
//                return this._Extension;
//            }
//            set
//            {
//                if ((this._Extension != value))
//                {
//                    this.OnExtensionChanging(value);
//                    this.SendPropertyChanging();
//                    this._Extension = value;
//                    this.SendPropertyChanged("Extension");
//                    this.OnExtensionChanged();
//                }
//            }
//        }
//    }

//    public partial class SupplierContact : FullContact
//    {

//        private string _HomePage;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnHomePageChanging(string value);
//        partial void OnHomePageChanged();
//        #endregion

//        public SupplierContact()
//        {
//            OnCreated();
//        }

//        [Column(Storage = "_HomePage", DbType = "NText", UpdateCheck = UpdateCheck.Never)]
//        public string HomePage
//        {
//            get
//            {
//                return this._HomePage;
//            }
//            set
//            {
//                if ((this._HomePage != value))
//                {
//                    this.OnHomePageChanging(value);
//                    this.SendPropertyChanging();
//                    this._HomePage = value;
//                    this.SendPropertyChanged("HomePage");
//                    this.OnHomePageChanged();
//                }
//            }
//        }
//    }

//    public partial class CustomerContact : FullContact
//    {

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        #endregion

//        public CustomerContact()
//        {
//            OnCreated();
//        }
//    }

//    public partial class ShipperContact : Contact
//    {

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        #endregion

//        public ShipperContact()
//        {
//            OnCreated();
//        }
//    }

//    [Table(Name = "dbo.Current Product List")]
//    public partial class CurrentProductList
//    {

//        private int _ProductID;

//        private string _ProductName;

//        public CurrentProductList()
//        {
//        }

//        [Column(Storage = "_ProductID", DbType = "Int NOT NULL IDENTITY")]
//        public int ProductID
//        {
//            get
//            {
//                return this._ProductID;
//            }
//            set
//            {
//                if ((this._ProductID != value))
//                {
//                    this._ProductID = value;
//                }
//            }
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this._ProductName = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Customer and Suppliers by City")]
//    public partial class CustomerAndSuppliersByCity
//    {

//        private string _City;

//        private string _CompanyName;

//        private string _ContactName;

//        private string _Relationship;

//        public CustomerAndSuppliersByCity()
//        {
//        }

//        [Column(Storage = "_City", DbType = "NVarChar(15)")]
//        public string City
//        {
//            get
//            {
//                return this._City;
//            }
//            set
//            {
//                if ((this._City != value))
//                {
//                    this._City = value;
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this._CompanyName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ContactName", DbType = "NVarChar(30)")]
//        public string ContactName
//        {
//            get
//            {
//                return this._ContactName;
//            }
//            set
//            {
//                if ((this._ContactName != value))
//                {
//                    this._ContactName = value;
//                }
//            }
//        }

//        [Column(Storage = "_Relationship", DbType = "VarChar(9) NOT NULL", CanBeNull = false)]
//        public string Relationship
//        {
//            get
//            {
//                return this._Relationship;
//            }
//            set
//            {
//                if ((this._Relationship != value))
//                {
//                    this._Relationship = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.CustomerCustomerDemo")]
//    public partial class CustomerCustomerDemo : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private string _CustomerID;

//        private string _CustomerTypeID;

//        private EntityRef<CustomerDemographic> _CustomerDemographic;

//        private EntityRef<Customer> _Customer;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnCustomerIDChanging(string value);
//        partial void OnCustomerIDChanged();
//        partial void OnCustomerTypeIDChanging(string value);
//        partial void OnCustomerTypeIDChanged();
//        #endregion

//        public CustomerCustomerDemo()
//        {
//            this._CustomerDemographic = default(EntityRef<CustomerDemographic>);
//            this._Customer = default(EntityRef<Customer>);
//            OnCreated();
//        }

//        [Column(Storage = "_CustomerID", DbType = "NChar(5) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
//        public string CustomerID
//        {
//            get
//            {
//                return this._CustomerID;
//            }
//            set
//            {
//                if ((this._CustomerID != value))
//                {
//                    if (this._Customer.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnCustomerIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._CustomerID = value;
//                    this.SendPropertyChanged("CustomerID");
//                    this.OnCustomerIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_CustomerTypeID", DbType = "NChar(10) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
//        public string CustomerTypeID
//        {
//            get
//            {
//                return this._CustomerTypeID;
//            }
//            set
//            {
//                if ((this._CustomerTypeID != value))
//                {
//                    if (this._CustomerDemographic.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnCustomerTypeIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._CustomerTypeID = value;
//                    this.SendPropertyChanged("CustomerTypeID");
//                    this.OnCustomerTypeIDChanged();
//                }
//            }
//        }

//        [Association(Name = "CustomerDemographic_CustomerCustomerDemo", Storage = "_CustomerDemographic", ThisKey = "CustomerTypeID", IsForeignKey = true)]
//        public CustomerDemographic CustomerDemographic
//        {
//            get
//            {
//                return this._CustomerDemographic.Entity;
//            }
//            set
//            {
//                CustomerDemographic previousValue = this._CustomerDemographic.Entity;
//                if (((previousValue != value)
//                            || (this._CustomerDemographic.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._CustomerDemographic.Entity = null;
//                        previousValue.CustomerCustomerDemos.Remove(this);
//                    }
//                    this._CustomerDemographic.Entity = value;
//                    if ((value != null))
//                    {
//                        value.CustomerCustomerDemos.Add(this);
//                        this._CustomerTypeID = value.CustomerTypeID;
//                    }
//                    else
//                    {
//                        this._CustomerTypeID = default(string);
//                    }
//                    this.SendPropertyChanged("CustomerDemographic");
//                }
//            }
//        }

//        [Association(Name = "Customer_CustomerCustomerDemo", Storage = "_Customer", ThisKey = "CustomerID", IsForeignKey = true)]
//        public Customer Customer
//        {
//            get
//            {
//                return this._Customer.Entity;
//            }
//            set
//            {
//                Customer previousValue = this._Customer.Entity;
//                if (((previousValue != value)
//                            || (this._Customer.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._Customer.Entity = null;
//                        previousValue.CustomerCustomerDemos.Remove(this);
//                    }
//                    this._Customer.Entity = value;
//                    if ((value != null))
//                    {
//                        value.CustomerCustomerDemos.Add(this);
//                        this._CustomerID = value.CustomerID;
//                    }
//                    else
//                    {
//                        this._CustomerID = default(string);
//                    }
//                    this.SendPropertyChanged("Customer");
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }

//    [Table(Name = "dbo.CustomerDemographics")]
//    public partial class CustomerDemographic : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private string _CustomerTypeID;

//        private string _CustomerDesc;

//        private EntitySet<CustomerCustomerDemo> _CustomerCustomerDemos;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnCustomerTypeIDChanging(string value);
//        partial void OnCustomerTypeIDChanged();
//        partial void OnCustomerDescChanging(string value);
//        partial void OnCustomerDescChanged();
//        #endregion

//        public CustomerDemographic()
//        {
//            this._CustomerCustomerDemos = new EntitySet<CustomerCustomerDemo>(new Action<CustomerCustomerDemo>(this.attach_CustomerCustomerDemos), new Action<CustomerCustomerDemo>(this.detach_CustomerCustomerDemos));
//            OnCreated();
//        }

//        [Column(Storage = "_CustomerTypeID", DbType = "NChar(10) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
//        public string CustomerTypeID
//        {
//            get
//            {
//                return this._CustomerTypeID;
//            }
//            set
//            {
//                if ((this._CustomerTypeID != value))
//                {
//                    this.OnCustomerTypeIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._CustomerTypeID = value;
//                    this.SendPropertyChanged("CustomerTypeID");
//                    this.OnCustomerTypeIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_CustomerDesc", DbType = "NText", UpdateCheck = UpdateCheck.Never)]
//        public string CustomerDesc
//        {
//            get
//            {
//                return this._CustomerDesc;
//            }
//            set
//            {
//                if ((this._CustomerDesc != value))
//                {
//                    this.OnCustomerDescChanging(value);
//                    this.SendPropertyChanging();
//                    this._CustomerDesc = value;
//                    this.SendPropertyChanged("CustomerDesc");
//                    this.OnCustomerDescChanged();
//                }
//            }
//        }

//        [Association(Name = "CustomerDemographic_CustomerCustomerDemo", Storage = "_CustomerCustomerDemos", OtherKey = "CustomerTypeID")]
//        public EntitySet<CustomerCustomerDemo> CustomerCustomerDemos
//        {
//            get
//            {
//                return this._CustomerCustomerDemos;
//            }
//            set
//            {
//                this._CustomerCustomerDemos.Assign(value);
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        private void attach_CustomerCustomerDemos(CustomerCustomerDemo entity)
//        {
//            this.SendPropertyChanging();
//            entity.CustomerDemographic = this;
//            this.SendPropertyChanged("CustomerCustomerDemos");
//        }

//        private void detach_CustomerCustomerDemos(CustomerCustomerDemo entity)
//        {
//            this.SendPropertyChanging();
//            entity.CustomerDemographic = null;
//            this.SendPropertyChanged("CustomerCustomerDemos");
//        }
//    }

//    [Table(Name = "dbo.Customers")]
//    public partial class Customer : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private string _CustomerID;

//        private string _CompanyName;

//        private string _ContactName;

//        private string _ContactTitle;

//        private string _Address;

//        private string _City;

//        private string _Region;

//        private string _PostalCode;

//        private string _Country;

//        private string _Phone;

//        private string _Fax;

//        private EntitySet<CustomerCustomerDemo> _CustomerCustomerDemos;

//        private EntitySet<Order> _Orders;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnCustomerIDChanging(string value);
//        partial void OnCustomerIDChanged();
//        partial void OnCompanyNameChanging(string value);
//        partial void OnCompanyNameChanged();
//        partial void OnContactNameChanging(string value);
//        partial void OnContactNameChanged();
//        partial void OnContactTitleChanging(string value);
//        partial void OnContactTitleChanged();
//        partial void OnAddressChanging(string value);
//        partial void OnAddressChanged();
//        partial void OnCityChanging(string value);
//        partial void OnCityChanged();
//        partial void OnRegionChanging(string value);
//        partial void OnRegionChanged();
//        partial void OnPostalCodeChanging(string value);
//        partial void OnPostalCodeChanged();
//        partial void OnCountryChanging(string value);
//        partial void OnCountryChanged();
//        partial void OnPhoneChanging(string value);
//        partial void OnPhoneChanged();
//        partial void OnFaxChanging(string value);
//        partial void OnFaxChanged();
//        #endregion

//        public Customer()
//        {
//            this._CustomerCustomerDemos = new EntitySet<CustomerCustomerDemo>(new Action<CustomerCustomerDemo>(this.attach_CustomerCustomerDemos), new Action<CustomerCustomerDemo>(this.detach_CustomerCustomerDemos));
//            this._Orders = new EntitySet<Order>(new Action<Order>(this.attach_Orders), new Action<Order>(this.detach_Orders));
//            OnCreated();
//        }

//        [Column(Storage = "_CustomerID", DbType = "NChar(5) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
//        public string CustomerID
//        {
//            get
//            {
//                return this._CustomerID;
//            }
//            set
//            {
//                if ((this._CustomerID != value))
//                {
//                    this.OnCustomerIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._CustomerID = value;
//                    this.SendPropertyChanged("CustomerID");
//                    this.OnCustomerIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this.OnCompanyNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._CompanyName = value;
//                    this.SendPropertyChanged("CompanyName");
//                    this.OnCompanyNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ContactName", DbType = "NVarChar(30)")]
//        public string ContactName
//        {
//            get
//            {
//                return this._ContactName;
//            }
//            set
//            {
//                if ((this._ContactName != value))
//                {
//                    this.OnContactNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._ContactName = value;
//                    this.SendPropertyChanged("ContactName");
//                    this.OnContactNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ContactTitle", DbType = "NVarChar(30)")]
//        public string ContactTitle
//        {
//            get
//            {
//                return this._ContactTitle;
//            }
//            set
//            {
//                if ((this._ContactTitle != value))
//                {
//                    this.OnContactTitleChanging(value);
//                    this.SendPropertyChanging();
//                    this._ContactTitle = value;
//                    this.SendPropertyChanged("ContactTitle");
//                    this.OnContactTitleChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Address", DbType = "NVarChar(60)")]
//        public string Address
//        {
//            get
//            {
//                return this._Address;
//            }
//            set
//            {
//                if ((this._Address != value))
//                {
//                    this.OnAddressChanging(value);
//                    this.SendPropertyChanging();
//                    this._Address = value;
//                    this.SendPropertyChanged("Address");
//                    this.OnAddressChanged();
//                }
//            }
//        }

//        [Column(Storage = "_City", DbType = "NVarChar(15)")]
//        public string City
//        {
//            get
//            {
//                return this._City;
//            }
//            set
//            {
//                if ((this._City != value))
//                {
//                    this.OnCityChanging(value);
//                    this.SendPropertyChanging();
//                    this._City = value;
//                    this.SendPropertyChanged("City");
//                    this.OnCityChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Region", DbType = "NVarChar(15)")]
//        public string Region
//        {
//            get
//            {
//                return this._Region;
//            }
//            set
//            {
//                if ((this._Region != value))
//                {
//                    this.OnRegionChanging(value);
//                    this.SendPropertyChanging();
//                    this._Region = value;
//                    this.SendPropertyChanged("Region");
//                    this.OnRegionChanged();
//                }
//            }
//        }

//        [Column(Storage = "_PostalCode", DbType = "NVarChar(10)")]
//        public string PostalCode
//        {
//            get
//            {
//                return this._PostalCode;
//            }
//            set
//            {
//                if ((this._PostalCode != value))
//                {
//                    this.OnPostalCodeChanging(value);
//                    this.SendPropertyChanging();
//                    this._PostalCode = value;
//                    this.SendPropertyChanged("PostalCode");
//                    this.OnPostalCodeChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Country", DbType = "NVarChar(15)")]
//        public string Country
//        {
//            get
//            {
//                return this._Country;
//            }
//            set
//            {
//                if ((this._Country != value))
//                {
//                    this.OnCountryChanging(value);
//                    this.SendPropertyChanging();
//                    this._Country = value;
//                    this.SendPropertyChanged("Country");
//                    this.OnCountryChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Phone", DbType = "NVarChar(24)")]
//        public string Phone
//        {
//            get
//            {
//                return this._Phone;
//            }
//            set
//            {
//                if ((this._Phone != value))
//                {
//                    this.OnPhoneChanging(value);
//                    this.SendPropertyChanging();
//                    this._Phone = value;
//                    this.SendPropertyChanged("Phone");
//                    this.OnPhoneChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Fax", DbType = "NVarChar(24)")]
//        public string Fax
//        {
//            get
//            {
//                return this._Fax;
//            }
//            set
//            {
//                if ((this._Fax != value))
//                {
//                    this.OnFaxChanging(value);
//                    this.SendPropertyChanging();
//                    this._Fax = value;
//                    this.SendPropertyChanged("Fax");
//                    this.OnFaxChanged();
//                }
//            }
//        }

//        [Association(Name = "Customer_CustomerCustomerDemo", Storage = "_CustomerCustomerDemos", OtherKey = "CustomerID")]
//        public EntitySet<CustomerCustomerDemo> CustomerCustomerDemos
//        {
//            get
//            {
//                return this._CustomerCustomerDemos;
//            }
//            set
//            {
//                this._CustomerCustomerDemos.Assign(value);
//            }
//        }

//        [Association(Name = "Customer_Order", Storage = "_Orders", OtherKey = "CustomerID")]
//        public EntitySet<Order> Orders
//        {
//            get
//            {
//                return this._Orders;
//            }
//            set
//            {
//                this._Orders.Assign(value);
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        private void attach_CustomerCustomerDemos(CustomerCustomerDemo entity)
//        {
//            this.SendPropertyChanging();
//            entity.Customer = this;
//            this.SendPropertyChanged("CustomerCustomerDemos");
//        }

//        private void detach_CustomerCustomerDemos(CustomerCustomerDemo entity)
//        {
//            this.SendPropertyChanging();
//            entity.Customer = null;
//            this.SendPropertyChanged("CustomerCustomerDemos");
//        }

//        private void attach_Orders(Order entity)
//        {
//            this.SendPropertyChanging();
//            entity.Customer = this;
//            this.SendPropertyChanged("Orders");
//        }

//        private void detach_Orders(Order entity)
//        {
//            this.SendPropertyChanging();
//            entity.Customer = null;
//            this.SendPropertyChanged("Orders");
//        }
//    }

//    [Table(Name = "dbo.DiscontinuedProductsFedarated")]
//    public partial class DiscontinuedProductsFedarated : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _ProductID;

//        private string _ProductName;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnProductIDChanging(int value);
//        partial void OnProductIDChanged();
//        partial void OnProductNameChanging(string value);
//        partial void OnProductNameChanged();
//        #endregion

//        public DiscontinuedProductsFedarated()
//        {
//            OnCreated();
//        }

//        [Column(Storage = "_ProductID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
//        public int ProductID
//        {
//            get
//            {
//                return this._ProductID;
//            }
//            set
//            {
//                if ((this._ProductID != value))
//                {
//                    this.OnProductIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._ProductID = value;
//                    this.SendPropertyChanged("ProductID");
//                    this.OnProductIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this.OnProductNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._ProductName = value;
//                    this.SendPropertyChanged("ProductName");
//                    this.OnProductNameChanged();
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }

//    [Table(Name = "dbo.Employees")]
//    public partial class Employee : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _EmployeeID;

//        private string _LastName;

//        private string _FirstName;

//        private string _Title;

//        private string _TitleOfCourtesy;

//        private System.Nullable<System.DateTime> _BirthDate;

//        private System.Nullable<System.DateTime> _HireDate;

//        private string _Address;

//        private string _City;

//        private string _Region;

//        private string _PostalCode;

//        private string _Country;

//        private string _HomePhone;

//        private string _Extension;

//        private System.Data.Linq.Binary _Photo;

//        private string _Notes;

//        private System.Nullable<int> _ReportsTo;

//        private string _PhotoPath;

//        private EntitySet<Employee> _Employees;

//        private EntitySet<EmployeeTerritory> _EmployeeTerritories;

//        private EntitySet<Order> _Orders;

//        private EntityRef<Employee> _ReportsToEmployee;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnEmployeeIDChanging(int value);
//        partial void OnEmployeeIDChanged();
//        partial void OnLastNameChanging(string value);
//        partial void OnLastNameChanged();
//        partial void OnFirstNameChanging(string value);
//        partial void OnFirstNameChanged();
//        partial void OnTitleChanging(string value);
//        partial void OnTitleChanged();
//        partial void OnTitleOfCourtesyChanging(string value);
//        partial void OnTitleOfCourtesyChanged();
//        partial void OnBirthDateChanging(System.Nullable<System.DateTime> value);
//        partial void OnBirthDateChanged();
//        partial void OnHireDateChanging(System.Nullable<System.DateTime> value);
//        partial void OnHireDateChanged();
//        partial void OnAddressChanging(string value);
//        partial void OnAddressChanged();
//        partial void OnCityChanging(string value);
//        partial void OnCityChanged();
//        partial void OnRegionChanging(string value);
//        partial void OnRegionChanged();
//        partial void OnPostalCodeChanging(string value);
//        partial void OnPostalCodeChanged();
//        partial void OnCountryChanging(string value);
//        partial void OnCountryChanged();
//        partial void OnHomePhoneChanging(string value);
//        partial void OnHomePhoneChanged();
//        partial void OnExtensionChanging(string value);
//        partial void OnExtensionChanged();
//        partial void OnPhotoChanging(System.Data.Linq.Binary value);
//        partial void OnPhotoChanged();
//        partial void OnNotesChanging(string value);
//        partial void OnNotesChanged();
//        partial void OnReportsToChanging(System.Nullable<int> value);
//        partial void OnReportsToChanged();
//        partial void OnPhotoPathChanging(string value);
//        partial void OnPhotoPathChanged();
//        #endregion

//        public Employee()
//        {
//            this._Employees = new EntitySet<Employee>(new Action<Employee>(this.attach_Employees), new Action<Employee>(this.detach_Employees));
//            this._EmployeeTerritories = new EntitySet<EmployeeTerritory>(new Action<EmployeeTerritory>(this.attach_EmployeeTerritories), new Action<EmployeeTerritory>(this.detach_EmployeeTerritories));
//            this._Orders = new EntitySet<Order>(new Action<Order>(this.attach_Orders), new Action<Order>(this.detach_Orders));
//            this._ReportsToEmployee = default(EntityRef<Employee>);
//            OnCreated();
//        }

//        [Column(Storage = "_EmployeeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
//        public int EmployeeID
//        {
//            get
//            {
//                return this._EmployeeID;
//            }
//            set
//            {
//                if ((this._EmployeeID != value))
//                {
//                    this.OnEmployeeIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._EmployeeID = value;
//                    this.SendPropertyChanged("EmployeeID");
//                    this.OnEmployeeIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_LastName", DbType = "NVarChar(20) NOT NULL", CanBeNull = false)]
//        public string LastName
//        {
//            get
//            {
//                return this._LastName;
//            }
//            set
//            {
//                if ((this._LastName != value))
//                {
//                    this.OnLastNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._LastName = value;
//                    this.SendPropertyChanged("LastName");
//                    this.OnLastNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_FirstName", DbType = "NVarChar(10) NOT NULL", CanBeNull = false)]
//        public string FirstName
//        {
//            get
//            {
//                return this._FirstName;
//            }
//            set
//            {
//                if ((this._FirstName != value))
//                {
//                    this.OnFirstNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._FirstName = value;
//                    this.SendPropertyChanged("FirstName");
//                    this.OnFirstNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Title", DbType = "NVarChar(30)")]
//        public string Title
//        {
//            get
//            {
//                return this._Title;
//            }
//            set
//            {
//                if ((this._Title != value))
//                {
//                    this.OnTitleChanging(value);
//                    this.SendPropertyChanging();
//                    this._Title = value;
//                    this.SendPropertyChanged("Title");
//                    this.OnTitleChanged();
//                }
//            }
//        }

//        [Column(Storage = "_TitleOfCourtesy", DbType = "NVarChar(25)")]
//        public string TitleOfCourtesy
//        {
//            get
//            {
//                return this._TitleOfCourtesy;
//            }
//            set
//            {
//                if ((this._TitleOfCourtesy != value))
//                {
//                    this.OnTitleOfCourtesyChanging(value);
//                    this.SendPropertyChanging();
//                    this._TitleOfCourtesy = value;
//                    this.SendPropertyChanged("TitleOfCourtesy");
//                    this.OnTitleOfCourtesyChanged();
//                }
//            }
//        }

//        [Column(Storage = "_BirthDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> BirthDate
//        {
//            get
//            {
//                return this._BirthDate;
//            }
//            set
//            {
//                if ((this._BirthDate != value))
//                {
//                    this.OnBirthDateChanging(value);
//                    this.SendPropertyChanging();
//                    this._BirthDate = value;
//                    this.SendPropertyChanged("BirthDate");
//                    this.OnBirthDateChanged();
//                }
//            }
//        }

//        [Column(Storage = "_HireDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> HireDate
//        {
//            get
//            {
//                return this._HireDate;
//            }
//            set
//            {
//                if ((this._HireDate != value))
//                {
//                    this.OnHireDateChanging(value);
//                    this.SendPropertyChanging();
//                    this._HireDate = value;
//                    this.SendPropertyChanged("HireDate");
//                    this.OnHireDateChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Address", DbType = "NVarChar(60)")]
//        public string Address
//        {
//            get
//            {
//                return this._Address;
//            }
//            set
//            {
//                if ((this._Address != value))
//                {
//                    this.OnAddressChanging(value);
//                    this.SendPropertyChanging();
//                    this._Address = value;
//                    this.SendPropertyChanged("Address");
//                    this.OnAddressChanged();
//                }
//            }
//        }

//        [Column(Storage = "_City", DbType = "NVarChar(15)")]
//        public string City
//        {
//            get
//            {
//                return this._City;
//            }
//            set
//            {
//                if ((this._City != value))
//                {
//                    this.OnCityChanging(value);
//                    this.SendPropertyChanging();
//                    this._City = value;
//                    this.SendPropertyChanged("City");
//                    this.OnCityChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Region", DbType = "NVarChar(15)")]
//        public string Region
//        {
//            get
//            {
//                return this._Region;
//            }
//            set
//            {
//                if ((this._Region != value))
//                {
//                    this.OnRegionChanging(value);
//                    this.SendPropertyChanging();
//                    this._Region = value;
//                    this.SendPropertyChanged("Region");
//                    this.OnRegionChanged();
//                }
//            }
//        }

//        [Column(Storage = "_PostalCode", DbType = "NVarChar(10)")]
//        public string PostalCode
//        {
//            get
//            {
//                return this._PostalCode;
//            }
//            set
//            {
//                if ((this._PostalCode != value))
//                {
//                    this.OnPostalCodeChanging(value);
//                    this.SendPropertyChanging();
//                    this._PostalCode = value;
//                    this.SendPropertyChanged("PostalCode");
//                    this.OnPostalCodeChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Country", DbType = "NVarChar(15)")]
//        public string Country
//        {
//            get
//            {
//                return this._Country;
//            }
//            set
//            {
//                if ((this._Country != value))
//                {
//                    this.OnCountryChanging(value);
//                    this.SendPropertyChanging();
//                    this._Country = value;
//                    this.SendPropertyChanged("Country");
//                    this.OnCountryChanged();
//                }
//            }
//        }

//        [Column(Storage = "_HomePhone", DbType = "NVarChar(24)")]
//        public string HomePhone
//        {
//            get
//            {
//                return this._HomePhone;
//            }
//            set
//            {
//                if ((this._HomePhone != value))
//                {
//                    this.OnHomePhoneChanging(value);
//                    this.SendPropertyChanging();
//                    this._HomePhone = value;
//                    this.SendPropertyChanged("HomePhone");
//                    this.OnHomePhoneChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Extension", DbType = "NVarChar(4)")]
//        public string Extension
//        {
//            get
//            {
//                return this._Extension;
//            }
//            set
//            {
//                if ((this._Extension != value))
//                {
//                    this.OnExtensionChanging(value);
//                    this.SendPropertyChanging();
//                    this._Extension = value;
//                    this.SendPropertyChanged("Extension");
//                    this.OnExtensionChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Photo", DbType = "Image", CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
//        public System.Data.Linq.Binary Photo
//        {
//            get
//            {
//                return this._Photo;
//            }
//            set
//            {
//                if ((this._Photo != value))
//                {
//                    this.OnPhotoChanging(value);
//                    this.SendPropertyChanging();
//                    this._Photo = value;
//                    this.SendPropertyChanged("Photo");
//                    this.OnPhotoChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Notes", DbType = "NText", UpdateCheck = UpdateCheck.Never)]
//        public string Notes
//        {
//            get
//            {
//                return this._Notes;
//            }
//            set
//            {
//                if ((this._Notes != value))
//                {
//                    this.OnNotesChanging(value);
//                    this.SendPropertyChanging();
//                    this._Notes = value;
//                    this.SendPropertyChanged("Notes");
//                    this.OnNotesChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ReportsTo", DbType = "Int")]
//        public System.Nullable<int> ReportsTo
//        {
//            get
//            {
//                return this._ReportsTo;
//            }
//            set
//            {
//                if ((this._ReportsTo != value))
//                {
//                    if (this._ReportsToEmployee.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnReportsToChanging(value);
//                    this.SendPropertyChanging();
//                    this._ReportsTo = value;
//                    this.SendPropertyChanged("ReportsTo");
//                    this.OnReportsToChanged();
//                }
//            }
//        }

//        [Column(Storage = "_PhotoPath", DbType = "NVarChar(255)")]
//        public string PhotoPath
//        {
//            get
//            {
//                return this._PhotoPath;
//            }
//            set
//            {
//                if ((this._PhotoPath != value))
//                {
//                    this.OnPhotoPathChanging(value);
//                    this.SendPropertyChanging();
//                    this._PhotoPath = value;
//                    this.SendPropertyChanged("PhotoPath");
//                    this.OnPhotoPathChanged();
//                }
//            }
//        }

//        [Association(Name = "Employee_Employee", Storage = "_Employees", OtherKey = "ReportsTo")]
//        public EntitySet<Employee> Employees
//        {
//            get
//            {
//                return this._Employees;
//            }
//            set
//            {
//                this._Employees.Assign(value);
//            }
//        }

//        [Association(Name = "Employee_EmployeeTerritory", Storage = "_EmployeeTerritories", OtherKey = "EmployeeID")]
//        public EntitySet<EmployeeTerritory> EmployeeTerritories
//        {
//            get
//            {
//                return this._EmployeeTerritories;
//            }
//            set
//            {
//                this._EmployeeTerritories.Assign(value);
//            }
//        }

//        [Association(Name = "Employee_Order", Storage = "_Orders", OtherKey = "EmployeeID")]
//        public EntitySet<Order> Orders
//        {
//            get
//            {
//                return this._Orders;
//            }
//            set
//            {
//                this._Orders.Assign(value);
//            }
//        }

//        [Association(Name = "Employee_Employee", Storage = "_ReportsToEmployee", ThisKey = "ReportsTo", IsForeignKey = true)]
//        public Employee ReportsToEmployee
//        {
//            get
//            {
//                return this._ReportsToEmployee.Entity;
//            }
//            set
//            {
//                Employee previousValue = this._ReportsToEmployee.Entity;
//                if (((previousValue != value)
//                            || (this._ReportsToEmployee.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._ReportsToEmployee.Entity = null;
//                        previousValue.Employees.Remove(this);
//                    }
//                    this._ReportsToEmployee.Entity = value;
//                    if ((value != null))
//                    {
//                        value.Employees.Add(this);
//                        this._ReportsTo = value.EmployeeID;
//                    }
//                    else
//                    {
//                        this._ReportsTo = default(Nullable<int>);
//                    }
//                    this.SendPropertyChanged("ReportsToEmployee");
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        private void attach_Employees(Employee entity)
//        {
//            this.SendPropertyChanging();
//            entity.ReportsToEmployee = this;
//            this.SendPropertyChanged("Employees");
//        }

//        private void detach_Employees(Employee entity)
//        {
//            this.SendPropertyChanging();
//            entity.ReportsToEmployee = null;
//            this.SendPropertyChanged("Employees");
//        }

//        private void attach_EmployeeTerritories(EmployeeTerritory entity)
//        {
//            this.SendPropertyChanging();
//            entity.Employee = this;
//            this.SendPropertyChanged("EmployeeTerritories");
//        }

//        private void detach_EmployeeTerritories(EmployeeTerritory entity)
//        {
//            this.SendPropertyChanging();
//            entity.Employee = null;
//            this.SendPropertyChanged("EmployeeTerritories");
//        }

//        private void attach_Orders(Order entity)
//        {
//            this.SendPropertyChanging();
//            entity.Employee = this;
//            this.SendPropertyChanged("Orders");
//        }

//        private void detach_Orders(Order entity)
//        {
//            this.SendPropertyChanging();
//            entity.Employee = null;
//            this.SendPropertyChanged("Orders");
//        }
//    }

//    [Table(Name = "dbo.EmployeeSplit")]
//    public partial class EmployeeSplit : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _ID;

//        private string _Extension;

//        private string _PhotoPath;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnIDChanging(int value);
//        partial void OnIDChanged();
//        partial void OnExtensionChanging(string value);
//        partial void OnExtensionChanged();
//        partial void OnPhotoPathChanging(string value);
//        partial void OnPhotoPathChanged();
//        #endregion

//        public EmployeeSplit()
//        {
//            OnCreated();
//        }

//        [Column(Storage = "_ID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
//        public int ID
//        {
//            get
//            {
//                return this._ID;
//            }
//            set
//            {
//                if ((this._ID != value))
//                {
//                    this.OnIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._ID = value;
//                    this.SendPropertyChanged("ID");
//                    this.OnIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Extension", DbType = "NVarChar(4)")]
//        public string Extension
//        {
//            get
//            {
//                return this._Extension;
//            }
//            set
//            {
//                if ((this._Extension != value))
//                {
//                    this.OnExtensionChanging(value);
//                    this.SendPropertyChanging();
//                    this._Extension = value;
//                    this.SendPropertyChanged("Extension");
//                    this.OnExtensionChanged();
//                }
//            }
//        }

//        [Column(Storage = "_PhotoPath", DbType = "NVarChar(255)")]
//        public string PhotoPath
//        {
//            get
//            {
//                return this._PhotoPath;
//            }
//            set
//            {
//                if ((this._PhotoPath != value))
//                {
//                    this.OnPhotoPathChanging(value);
//                    this.SendPropertyChanging();
//                    this._PhotoPath = value;
//                    this.SendPropertyChanged("PhotoPath");
//                    this.OnPhotoPathChanged();
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }

//    [Table(Name = "dbo.EmployeeTerritories")]
//    public partial class EmployeeTerritory : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _EmployeeID;

//        private string _TerritoryID;

//        private EntityRef<Employee> _Employee;

//        private EntityRef<Territory> _Territory;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnEmployeeIDChanging(int value);
//        partial void OnEmployeeIDChanged();
//        partial void OnTerritoryIDChanging(string value);
//        partial void OnTerritoryIDChanged();
//        #endregion

//        public EmployeeTerritory()
//        {
//            this._Employee = default(EntityRef<Employee>);
//            this._Territory = default(EntityRef<Territory>);
//            OnCreated();
//        }

//        [Column(Storage = "_EmployeeID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
//        public int EmployeeID
//        {
//            get
//            {
//                return this._EmployeeID;
//            }
//            set
//            {
//                if ((this._EmployeeID != value))
//                {
//                    if (this._Employee.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnEmployeeIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._EmployeeID = value;
//                    this.SendPropertyChanged("EmployeeID");
//                    this.OnEmployeeIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_TerritoryID", DbType = "NVarChar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
//        public string TerritoryID
//        {
//            get
//            {
//                return this._TerritoryID;
//            }
//            set
//            {
//                if ((this._TerritoryID != value))
//                {
//                    if (this._Territory.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnTerritoryIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._TerritoryID = value;
//                    this.SendPropertyChanged("TerritoryID");
//                    this.OnTerritoryIDChanged();
//                }
//            }
//        }

//        [Association(Name = "Employee_EmployeeTerritory", Storage = "_Employee", ThisKey = "EmployeeID", IsForeignKey = true)]
//        public Employee Employee
//        {
//            get
//            {
//                return this._Employee.Entity;
//            }
//            set
//            {
//                Employee previousValue = this._Employee.Entity;
//                if (((previousValue != value)
//                            || (this._Employee.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._Employee.Entity = null;
//                        previousValue.EmployeeTerritories.Remove(this);
//                    }
//                    this._Employee.Entity = value;
//                    if ((value != null))
//                    {
//                        value.EmployeeTerritories.Add(this);
//                        this._EmployeeID = value.EmployeeID;
//                    }
//                    else
//                    {
//                        this._EmployeeID = default(int);
//                    }
//                    this.SendPropertyChanged("Employee");
//                }
//            }
//        }

//        [Association(Name = "Territory_EmployeeTerritory", Storage = "_Territory", ThisKey = "TerritoryID", IsForeignKey = true)]
//        public Territory Territory
//        {
//            get
//            {
//                return this._Territory.Entity;
//            }
//            set
//            {
//                Territory previousValue = this._Territory.Entity;
//                if (((previousValue != value)
//                            || (this._Territory.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._Territory.Entity = null;
//                        previousValue.EmployeeTerritories.Remove(this);
//                    }
//                    this._Territory.Entity = value;
//                    if ((value != null))
//                    {
//                        value.EmployeeTerritories.Add(this);
//                        this._TerritoryID = value.TerritoryID;
//                    }
//                    else
//                    {
//                        this._TerritoryID = default(string);
//                    }
//                    this.SendPropertyChanged("Territory");
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }

//    [Table(Name = "dbo.Invoices")]
//    public partial class Invoices
//    {

//        private string _ShipName;

//        private string _ShipAddress;

//        private string _ShipCity;

//        private string _ShipRegion;

//        private string _ShipPostalCode;

//        private string _ShipCountry;

//        private string _CustomerID;

//        private string _CustomerName;

//        private string _Address;

//        private string _City;

//        private string _Region;

//        private string _PostalCode;

//        private string _Country;

//        private string _Salesperson;

//        private int _OrderID;

//        private System.Nullable<System.DateTime> _OrderDate;

//        private System.Nullable<System.DateTime> _RequiredDate;

//        private System.Nullable<System.DateTime> _ShippedDate;

//        private string _ShipperName;

//        private int _ProductID;

//        private string _ProductName;

//        private decimal _UnitPrice;

//        private short _Quantity;

//        private float _Discount;

//        private System.Nullable<decimal> _ExtendedPrice;

//        private System.Nullable<decimal> _Freight;

//        public Invoices()
//        {
//        }

//        [Column(Storage = "_ShipName", DbType = "NVarChar(40)")]
//        public string ShipName
//        {
//            get
//            {
//                return this._ShipName;
//            }
//            set
//            {
//                if ((this._ShipName != value))
//                {
//                    this._ShipName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipAddress", DbType = "NVarChar(60)")]
//        public string ShipAddress
//        {
//            get
//            {
//                return this._ShipAddress;
//            }
//            set
//            {
//                if ((this._ShipAddress != value))
//                {
//                    this._ShipAddress = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipCity", DbType = "NVarChar(15)")]
//        public string ShipCity
//        {
//            get
//            {
//                return this._ShipCity;
//            }
//            set
//            {
//                if ((this._ShipCity != value))
//                {
//                    this._ShipCity = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipRegion", DbType = "NVarChar(15)")]
//        public string ShipRegion
//        {
//            get
//            {
//                return this._ShipRegion;
//            }
//            set
//            {
//                if ((this._ShipRegion != value))
//                {
//                    this._ShipRegion = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipPostalCode", DbType = "NVarChar(10)")]
//        public string ShipPostalCode
//        {
//            get
//            {
//                return this._ShipPostalCode;
//            }
//            set
//            {
//                if ((this._ShipPostalCode != value))
//                {
//                    this._ShipPostalCode = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipCountry", DbType = "NVarChar(15)")]
//        public string ShipCountry
//        {
//            get
//            {
//                return this._ShipCountry;
//            }
//            set
//            {
//                if ((this._ShipCountry != value))
//                {
//                    this._ShipCountry = value;
//                }
//            }
//        }

//        [Column(Storage = "_CustomerID", DbType = "NChar(5)")]
//        public string CustomerID
//        {
//            get
//            {
//                return this._CustomerID;
//            }
//            set
//            {
//                if ((this._CustomerID != value))
//                {
//                    this._CustomerID = value;
//                }
//            }
//        }

//        [Column(Storage = "_CustomerName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string CustomerName
//        {
//            get
//            {
//                return this._CustomerName;
//            }
//            set
//            {
//                if ((this._CustomerName != value))
//                {
//                    this._CustomerName = value;
//                }
//            }
//        }

//        [Column(Storage = "_Address", DbType = "NVarChar(60)")]
//        public string Address
//        {
//            get
//            {
//                return this._Address;
//            }
//            set
//            {
//                if ((this._Address != value))
//                {
//                    this._Address = value;
//                }
//            }
//        }

//        [Column(Storage = "_City", DbType = "NVarChar(15)")]
//        public string City
//        {
//            get
//            {
//                return this._City;
//            }
//            set
//            {
//                if ((this._City != value))
//                {
//                    this._City = value;
//                }
//            }
//        }

//        [Column(Storage = "_Region", DbType = "NVarChar(15)")]
//        public string Region
//        {
//            get
//            {
//                return this._Region;
//            }
//            set
//            {
//                if ((this._Region != value))
//                {
//                    this._Region = value;
//                }
//            }
//        }

//        [Column(Storage = "_PostalCode", DbType = "NVarChar(10)")]
//        public string PostalCode
//        {
//            get
//            {
//                return this._PostalCode;
//            }
//            set
//            {
//                if ((this._PostalCode != value))
//                {
//                    this._PostalCode = value;
//                }
//            }
//        }

//        [Column(Storage = "_Country", DbType = "NVarChar(15)")]
//        public string Country
//        {
//            get
//            {
//                return this._Country;
//            }
//            set
//            {
//                if ((this._Country != value))
//                {
//                    this._Country = value;
//                }
//            }
//        }

//        [Column(Storage = "_Salesperson", DbType = "NVarChar(31) NOT NULL", CanBeNull = false)]
//        public string Salesperson
//        {
//            get
//            {
//                return this._Salesperson;
//            }
//            set
//            {
//                if ((this._Salesperson != value))
//                {
//                    this._Salesperson = value;
//                }
//            }
//        }

//        [Column(Storage = "_OrderID", DbType = "Int NOT NULL IDENTITY")]
//        public int OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    this._OrderID = value;
//                }
//            }
//        }

//        [Column(Storage = "_OrderDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> OrderDate
//        {
//            get
//            {
//                return this._OrderDate;
//            }
//            set
//            {
//                if ((this._OrderDate != value))
//                {
//                    this._OrderDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_RequiredDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> RequiredDate
//        {
//            get
//            {
//                return this._RequiredDate;
//            }
//            set
//            {
//                if ((this._RequiredDate != value))
//                {
//                    this._RequiredDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShippedDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> ShippedDate
//        {
//            get
//            {
//                return this._ShippedDate;
//            }
//            set
//            {
//                if ((this._ShippedDate != value))
//                {
//                    this._ShippedDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipperName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ShipperName
//        {
//            get
//            {
//                return this._ShipperName;
//            }
//            set
//            {
//                if ((this._ShipperName != value))
//                {
//                    this._ShipperName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ProductID", DbType = "Int NOT NULL")]
//        public int ProductID
//        {
//            get
//            {
//                return this._ProductID;
//            }
//            set
//            {
//                if ((this._ProductID != value))
//                {
//                    this._ProductID = value;
//                }
//            }
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this._ProductName = value;
//                }
//            }
//        }

//        [Column(Storage = "_UnitPrice", DbType = "Money NOT NULL")]
//        public decimal UnitPrice
//        {
//            get
//            {
//                return this._UnitPrice;
//            }
//            set
//            {
//                if ((this._UnitPrice != value))
//                {
//                    this._UnitPrice = value;
//                }
//            }
//        }

//        [Column(Storage = "_Quantity", DbType = "SmallInt NOT NULL")]
//        public short Quantity
//        {
//            get
//            {
//                return this._Quantity;
//            }
//            set
//            {
//                if ((this._Quantity != value))
//                {
//                    this._Quantity = value;
//                }
//            }
//        }

//        [Column(Storage = "_Discount", DbType = "Real NOT NULL")]
//        public float Discount
//        {
//            get
//            {
//                return this._Discount;
//            }
//            set
//            {
//                if ((this._Discount != value))
//                {
//                    this._Discount = value;
//                }
//            }
//        }

//        [Column(Storage = "_ExtendedPrice", DbType = "Money")]
//        public System.Nullable<decimal> ExtendedPrice
//        {
//            get
//            {
//                return this._ExtendedPrice;
//            }
//            set
//            {
//                if ((this._ExtendedPrice != value))
//                {
//                    this._ExtendedPrice = value;
//                }
//            }
//        }

//        [Column(Storage = "_Freight", DbType = "Money")]
//        public System.Nullable<decimal> Freight
//        {
//            get
//            {
//                return this._Freight;
//            }
//            set
//            {
//                if ((this._Freight != value))
//                {
//                    this._Freight = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Order Details")]
//    public partial class OrderDetail : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _OrderID;

//        private int _ProductID;

//        private decimal _UnitPrice;

//        private short _Quantity;

//        private float _Discount;

//        private EntityRef<Order> _Order;

//        private EntityRef<Product> _Product;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnOrderIDChanging(int value);
//        partial void OnOrderIDChanged();
//        partial void OnProductIDChanging(int value);
//        partial void OnProductIDChanged();
//        partial void OnUnitPriceChanging(decimal value);
//        partial void OnUnitPriceChanged();
//        partial void OnQuantityChanging(short value);
//        partial void OnQuantityChanged();
//        partial void OnDiscountChanging(float value);
//        partial void OnDiscountChanged();
//        #endregion

//        public OrderDetail()
//        {
//            this._Order = default(EntityRef<Order>);
//            this._Product = default(EntityRef<Product>);
//            OnCreated();
//        }

//        [Column(Storage = "_OrderID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
//        public int OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    if (this._Order.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnOrderIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._OrderID = value;
//                    this.SendPropertyChanged("OrderID");
//                    this.OnOrderIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ProductID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
//        public int ProductID
//        {
//            get
//            {
//                return this._ProductID;
//            }
//            set
//            {
//                if ((this._ProductID != value))
//                {
//                    if (this._Product.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnProductIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._ProductID = value;
//                    this.SendPropertyChanged("ProductID");
//                    this.OnProductIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_UnitPrice", DbType = "Money NOT NULL")]
//        public decimal UnitPrice
//        {
//            get
//            {
//                return this._UnitPrice;
//            }
//            set
//            {
//                if ((this._UnitPrice != value))
//                {
//                    this.OnUnitPriceChanging(value);
//                    this.SendPropertyChanging();
//                    this._UnitPrice = value;
//                    this.SendPropertyChanged("UnitPrice");
//                    this.OnUnitPriceChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Quantity", DbType = "SmallInt NOT NULL")]
//        public short Quantity
//        {
//            get
//            {
//                return this._Quantity;
//            }
//            set
//            {
//                if ((this._Quantity != value))
//                {
//                    this.OnQuantityChanging(value);
//                    this.SendPropertyChanging();
//                    this._Quantity = value;
//                    this.SendPropertyChanged("Quantity");
//                    this.OnQuantityChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Discount", DbType = "Real NOT NULL")]
//        public float Discount
//        {
//            get
//            {
//                return this._Discount;
//            }
//            set
//            {
//                if ((this._Discount != value))
//                {
//                    this.OnDiscountChanging(value);
//                    this.SendPropertyChanging();
//                    this._Discount = value;
//                    this.SendPropertyChanged("Discount");
//                    this.OnDiscountChanged();
//                }
//            }
//        }

//        [Association(Name = "Order_OrderDetail", Storage = "_Order", ThisKey = "OrderID", IsForeignKey = true, DeleteOnNull = true)]
//        public Order Order
//        {
//            get
//            {
//                return this._Order.Entity;
//            }
//            set
//            {
//                Order previousValue = this._Order.Entity;
//                if (((previousValue != value)
//                            || (this._Order.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._Order.Entity = null;
//                        previousValue.OrderDetails.Remove(this);
//                    }
//                    this._Order.Entity = value;
//                    if ((value != null))
//                    {
//                        value.OrderDetails.Add(this);
//                        this._OrderID = value.OrderID;
//                    }
//                    else
//                    {
//                        this._OrderID = default(int);
//                    }
//                    this.SendPropertyChanged("Order");
//                }
//            }
//        }

//        [Association(Name = "Product_OrderDetail", Storage = "_Product", ThisKey = "ProductID", IsForeignKey = true)]
//        public Product Product
//        {
//            get
//            {
//                return this._Product.Entity;
//            }
//            set
//            {
//                Product previousValue = this._Product.Entity;
//                if (((previousValue != value)
//                            || (this._Product.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._Product.Entity = null;
//                        previousValue.OrderDetails.Remove(this);
//                    }
//                    this._Product.Entity = value;
//                    if ((value != null))
//                    {
//                        value.OrderDetails.Add(this);
//                        this._ProductID = value.ProductID;
//                    }
//                    else
//                    {
//                        this._ProductID = default(int);
//                    }
//                    this.SendPropertyChanged("Product");
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }

//    [Table(Name = "dbo.Order Details Extended")]
//    public partial class OrderDetailsExtended
//    {

//        private int _OrderID;

//        private int _ProductID;

//        private string _ProductName;

//        private decimal _UnitPrice;

//        private short _Quantity;

//        private float _Discount;

//        private System.Nullable<decimal> _ExtendedPrice;

//        public OrderDetailsExtended()
//        {
//        }

//        [Column(Storage = "_OrderID", DbType = "Int NOT NULL")]
//        public int OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    this._OrderID = value;
//                }
//            }
//        }

//        [Column(Storage = "_ProductID", DbType = "Int NOT NULL")]
//        public int ProductID
//        {
//            get
//            {
//                return this._ProductID;
//            }
//            set
//            {
//                if ((this._ProductID != value))
//                {
//                    this._ProductID = value;
//                }
//            }
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this._ProductName = value;
//                }
//            }
//        }

//        [Column(Storage = "_UnitPrice", DbType = "Money NOT NULL")]
//        public decimal UnitPrice
//        {
//            get
//            {
//                return this._UnitPrice;
//            }
//            set
//            {
//                if ((this._UnitPrice != value))
//                {
//                    this._UnitPrice = value;
//                }
//            }
//        }

//        [Column(Storage = "_Quantity", DbType = "SmallInt NOT NULL")]
//        public short Quantity
//        {
//            get
//            {
//                return this._Quantity;
//            }
//            set
//            {
//                if ((this._Quantity != value))
//                {
//                    this._Quantity = value;
//                }
//            }
//        }

//        [Column(Storage = "_Discount", DbType = "Real NOT NULL")]
//        public float Discount
//        {
//            get
//            {
//                return this._Discount;
//            }
//            set
//            {
//                if ((this._Discount != value))
//                {
//                    this._Discount = value;
//                }
//            }
//        }

//        [Column(Storage = "_ExtendedPrice", DbType = "Money")]
//        public System.Nullable<decimal> ExtendedPrice
//        {
//            get
//            {
//                return this._ExtendedPrice;
//            }
//            set
//            {
//                if ((this._ExtendedPrice != value))
//                {
//                    this._ExtendedPrice = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Order Subtotals")]
//    public partial class OrderSubtotal
//    {

//        private int _OrderID;

//        private System.Nullable<decimal> _Subtotal;

//        public OrderSubtotal()
//        {
//        }

//        [Column(Storage = "_OrderID", DbType = "Int NOT NULL")]
//        public int OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    this._OrderID = value;
//                }
//            }
//        }

//        [Column(Storage = "_Subtotal", DbType = "Money")]
//        public System.Nullable<decimal> Subtotal
//        {
//            get
//            {
//                return this._Subtotal;
//            }
//            set
//            {
//                if ((this._Subtotal != value))
//                {
//                    this._Subtotal = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Orders")]
//    public partial class Order : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _OrderID;

//        private string _CustomerID;

//        private System.Nullable<int> _EmployeeID;

//        private System.Nullable<System.DateTime> _OrderDate;

//        private System.Nullable<System.DateTime> _RequiredDate;

//        private System.Nullable<System.DateTime> _ShippedDate;

//        private System.Nullable<int> _ShipVia;

//        private System.Nullable<decimal> _Freight;

//        private string _ShipName;

//        private string _ShipAddress;

//        private string _ShipCity;

//        private string _ShipRegion;

//        private string _ShipPostalCode;

//        private string _ShipCountry;

//        private EntitySet<OrderDetail> _OrderDetails;

//        private EntityRef<Customer> _Customer;

//        private EntityRef<Employee> _Employee;

//        private EntityRef<Shipper> _Shipper;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnOrderIDChanging(int value);
//        partial void OnOrderIDChanged();
//        partial void OnCustomerIDChanging(string value);
//        partial void OnCustomerIDChanged();
//        partial void OnEmployeeIDChanging(System.Nullable<int> value);
//        partial void OnEmployeeIDChanged();
//        partial void OnOrderDateChanging(System.Nullable<System.DateTime> value);
//        partial void OnOrderDateChanged();
//        partial void OnRequiredDateChanging(System.Nullable<System.DateTime> value);
//        partial void OnRequiredDateChanged();
//        partial void OnShippedDateChanging(System.Nullable<System.DateTime> value);
//        partial void OnShippedDateChanged();
//        partial void OnShipViaChanging(System.Nullable<int> value);
//        partial void OnShipViaChanged();
//        partial void OnFreightChanging(System.Nullable<decimal> value);
//        partial void OnFreightChanged();
//        partial void OnShipNameChanging(string value);
//        partial void OnShipNameChanged();
//        partial void OnShipAddressChanging(string value);
//        partial void OnShipAddressChanged();
//        partial void OnShipCityChanging(string value);
//        partial void OnShipCityChanged();
//        partial void OnShipRegionChanging(string value);
//        partial void OnShipRegionChanged();
//        partial void OnShipPostalCodeChanging(string value);
//        partial void OnShipPostalCodeChanged();
//        partial void OnShipCountryChanging(string value);
//        partial void OnShipCountryChanged();
//        #endregion

//        public Order()
//        {
//            this._OrderDetails = new EntitySet<OrderDetail>(new Action<OrderDetail>(this.attach_OrderDetails), new Action<OrderDetail>(this.detach_OrderDetails));
//            this._Customer = default(EntityRef<Customer>);
//            this._Employee = default(EntityRef<Employee>);
//            this._Shipper = default(EntityRef<Shipper>);
//            OnCreated();
//        }

//        [Column(Storage = "_OrderID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
//        public int OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    this.OnOrderIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._OrderID = value;
//                    this.SendPropertyChanged("OrderID");
//                    this.OnOrderIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_CustomerID", DbType = "NChar(5)")]
//        public string CustomerID
//        {
//            get
//            {
//                return this._CustomerID;
//            }
//            set
//            {
//                if ((this._CustomerID != value))
//                {
//                    if (this._Customer.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnCustomerIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._CustomerID = value;
//                    this.SendPropertyChanged("CustomerID");
//                    this.OnCustomerIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_EmployeeID", DbType = "Int")]
//        public System.Nullable<int> EmployeeID
//        {
//            get
//            {
//                return this._EmployeeID;
//            }
//            set
//            {
//                if ((this._EmployeeID != value))
//                {
//                    if (this._Employee.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnEmployeeIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._EmployeeID = value;
//                    this.SendPropertyChanged("EmployeeID");
//                    this.OnEmployeeIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_OrderDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> OrderDate
//        {
//            get
//            {
//                return this._OrderDate;
//            }
//            set
//            {
//                if ((this._OrderDate != value))
//                {
//                    this.OnOrderDateChanging(value);
//                    this.SendPropertyChanging();
//                    this._OrderDate = value;
//                    this.SendPropertyChanged("OrderDate");
//                    this.OnOrderDateChanged();
//                }
//            }
//        }

//        [Column(Storage = "_RequiredDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> RequiredDate
//        {
//            get
//            {
//                return this._RequiredDate;
//            }
//            set
//            {
//                if ((this._RequiredDate != value))
//                {
//                    this.OnRequiredDateChanging(value);
//                    this.SendPropertyChanging();
//                    this._RequiredDate = value;
//                    this.SendPropertyChanged("RequiredDate");
//                    this.OnRequiredDateChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ShippedDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> ShippedDate
//        {
//            get
//            {
//                return this._ShippedDate;
//            }
//            set
//            {
//                if ((this._ShippedDate != value))
//                {
//                    this.OnShippedDateChanging(value);
//                    this.SendPropertyChanging();
//                    this._ShippedDate = value;
//                    this.SendPropertyChanged("ShippedDate");
//                    this.OnShippedDateChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ShipVia", DbType = "Int")]
//        public System.Nullable<int> ShipVia
//        {
//            get
//            {
//                return this._ShipVia;
//            }
//            set
//            {
//                if ((this._ShipVia != value))
//                {
//                    if (this._Shipper.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnShipViaChanging(value);
//                    this.SendPropertyChanging();
//                    this._ShipVia = value;
//                    this.SendPropertyChanged("ShipVia");
//                    this.OnShipViaChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Freight", DbType = "Money")]
//        public System.Nullable<decimal> Freight
//        {
//            get
//            {
//                return this._Freight;
//            }
//            set
//            {
//                if ((this._Freight != value))
//                {
//                    this.OnFreightChanging(value);
//                    this.SendPropertyChanging();
//                    this._Freight = value;
//                    this.SendPropertyChanged("Freight");
//                    this.OnFreightChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ShipName", DbType = "NVarChar(40)")]
//        public string ShipName
//        {
//            get
//            {
//                return this._ShipName;
//            }
//            set
//            {
//                if ((this._ShipName != value))
//                {
//                    this.OnShipNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._ShipName = value;
//                    this.SendPropertyChanged("ShipName");
//                    this.OnShipNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ShipAddress", DbType = "NVarChar(60)")]
//        public string ShipAddress
//        {
//            get
//            {
//                return this._ShipAddress;
//            }
//            set
//            {
//                if ((this._ShipAddress != value))
//                {
//                    this.OnShipAddressChanging(value);
//                    this.SendPropertyChanging();
//                    this._ShipAddress = value;
//                    this.SendPropertyChanged("ShipAddress");
//                    this.OnShipAddressChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ShipCity", DbType = "NVarChar(15)")]
//        public string ShipCity
//        {
//            get
//            {
//                return this._ShipCity;
//            }
//            set
//            {
//                if ((this._ShipCity != value))
//                {
//                    this.OnShipCityChanging(value);
//                    this.SendPropertyChanging();
//                    this._ShipCity = value;
//                    this.SendPropertyChanged("ShipCity");
//                    this.OnShipCityChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ShipRegion", DbType = "NVarChar(15)")]
//        public string ShipRegion
//        {
//            get
//            {
//                return this._ShipRegion;
//            }
//            set
//            {
//                if ((this._ShipRegion != value))
//                {
//                    this.OnShipRegionChanging(value);
//                    this.SendPropertyChanging();
//                    this._ShipRegion = value;
//                    this.SendPropertyChanged("ShipRegion");
//                    this.OnShipRegionChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ShipPostalCode", DbType = "NVarChar(10)")]
//        public string ShipPostalCode
//        {
//            get
//            {
//                return this._ShipPostalCode;
//            }
//            set
//            {
//                if ((this._ShipPostalCode != value))
//                {
//                    this.OnShipPostalCodeChanging(value);
//                    this.SendPropertyChanging();
//                    this._ShipPostalCode = value;
//                    this.SendPropertyChanged("ShipPostalCode");
//                    this.OnShipPostalCodeChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ShipCountry", DbType = "NVarChar(15)")]
//        public string ShipCountry
//        {
//            get
//            {
//                return this._ShipCountry;
//            }
//            set
//            {
//                if ((this._ShipCountry != value))
//                {
//                    this.OnShipCountryChanging(value);
//                    this.SendPropertyChanging();
//                    this._ShipCountry = value;
//                    this.SendPropertyChanged("ShipCountry");
//                    this.OnShipCountryChanged();
//                }
//            }
//        }

//        [Association(Name = "Order_OrderDetail", Storage = "_OrderDetails", OtherKey = "OrderID")]
//        public EntitySet<OrderDetail> OrderDetails
//        {
//            get
//            {
//                return this._OrderDetails;
//            }
//            set
//            {
//                this._OrderDetails.Assign(value);
//            }
//        }

//        [Association(Name = "Customer_Order", Storage = "_Customer", ThisKey = "CustomerID", IsForeignKey = true)]
//        public Customer Customer
//        {
//            get
//            {
//                return this._Customer.Entity;
//            }
//            set
//            {
//                Customer previousValue = this._Customer.Entity;
//                if (((previousValue != value)
//                            || (this._Customer.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._Customer.Entity = null;
//                        previousValue.Orders.Remove(this);
//                    }
//                    this._Customer.Entity = value;
//                    if ((value != null))
//                    {
//                        value.Orders.Add(this);
//                        this._CustomerID = value.CustomerID;
//                    }
//                    else
//                    {
//                        this._CustomerID = default(string);
//                    }
//                    this.SendPropertyChanged("Customer");
//                }
//            }
//        }

//        [Association(Name = "Employee_Order", Storage = "_Employee", ThisKey = "EmployeeID", IsForeignKey = true)]
//        public Employee Employee
//        {
//            get
//            {
//                return this._Employee.Entity;
//            }
//            set
//            {
//                Employee previousValue = this._Employee.Entity;
//                if (((previousValue != value)
//                            || (this._Employee.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._Employee.Entity = null;
//                        previousValue.Orders.Remove(this);
//                    }
//                    this._Employee.Entity = value;
//                    if ((value != null))
//                    {
//                        value.Orders.Add(this);
//                        this._EmployeeID = value.EmployeeID;
//                    }
//                    else
//                    {
//                        this._EmployeeID = default(Nullable<int>);
//                    }
//                    this.SendPropertyChanged("Employee");
//                }
//            }
//        }

//        [Association(Name = "Shipper_Order", Storage = "_Shipper", ThisKey = "ShipVia", IsForeignKey = true)]
//        public Shipper Shipper
//        {
//            get
//            {
//                return this._Shipper.Entity;
//            }
//            set
//            {
//                Shipper previousValue = this._Shipper.Entity;
//                if (((previousValue != value)
//                            || (this._Shipper.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._Shipper.Entity = null;
//                        previousValue.Orders.Remove(this);
//                    }
//                    this._Shipper.Entity = value;
//                    if ((value != null))
//                    {
//                        value.Orders.Add(this);
//                        this._ShipVia = value.ShipperID;
//                    }
//                    else
//                    {
//                        this._ShipVia = default(Nullable<int>);
//                    }
//                    this.SendPropertyChanged("Shipper");
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        private void attach_OrderDetails(OrderDetail entity)
//        {
//            this.SendPropertyChanging();
//            entity.Order = this;
//            this.SendPropertyChanged("OrderDetails");
//        }

//        private void detach_OrderDetails(OrderDetail entity)
//        {
//            this.SendPropertyChanging();
//            entity.Order = null;
//            this.SendPropertyChanged("OrderDetails");
//        }
//    }

//    [Table(Name = "dbo.Orders Qry")]
//    public partial class OrdersQry
//    {

//        private int _OrderID;

//        private string _CustomerID;

//        private System.Nullable<int> _EmployeeID;

//        private System.Nullable<System.DateTime> _OrderDate;

//        private System.Nullable<System.DateTime> _RequiredDate;

//        private System.Nullable<System.DateTime> _ShippedDate;

//        private System.Nullable<int> _ShipVia;

//        private System.Nullable<decimal> _Freight;

//        private string _ShipName;

//        private string _ShipAddress;

//        private string _ShipCity;

//        private string _ShipRegion;

//        private string _ShipPostalCode;

//        private string _ShipCountry;

//        private string _CompanyName;

//        private string _Address;

//        private string _City;

//        private string _Region;

//        private string _PostalCode;

//        private string _Country;

//        public OrdersQry()
//        {
//        }

//        [Column(Storage = "_OrderID", DbType = "Int NOT NULL IDENTITY")]
//        public int OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    this._OrderID = value;
//                }
//            }
//        }

//        [Column(Storage = "_CustomerID", DbType = "NChar(5)")]
//        public string CustomerID
//        {
//            get
//            {
//                return this._CustomerID;
//            }
//            set
//            {
//                if ((this._CustomerID != value))
//                {
//                    this._CustomerID = value;
//                }
//            }
//        }

//        [Column(Storage = "_EmployeeID", DbType = "Int")]
//        public System.Nullable<int> EmployeeID
//        {
//            get
//            {
//                return this._EmployeeID;
//            }
//            set
//            {
//                if ((this._EmployeeID != value))
//                {
//                    this._EmployeeID = value;
//                }
//            }
//        }

//        [Column(Storage = "_OrderDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> OrderDate
//        {
//            get
//            {
//                return this._OrderDate;
//            }
//            set
//            {
//                if ((this._OrderDate != value))
//                {
//                    this._OrderDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_RequiredDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> RequiredDate
//        {
//            get
//            {
//                return this._RequiredDate;
//            }
//            set
//            {
//                if ((this._RequiredDate != value))
//                {
//                    this._RequiredDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShippedDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> ShippedDate
//        {
//            get
//            {
//                return this._ShippedDate;
//            }
//            set
//            {
//                if ((this._ShippedDate != value))
//                {
//                    this._ShippedDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipVia", DbType = "Int")]
//        public System.Nullable<int> ShipVia
//        {
//            get
//            {
//                return this._ShipVia;
//            }
//            set
//            {
//                if ((this._ShipVia != value))
//                {
//                    this._ShipVia = value;
//                }
//            }
//        }

//        [Column(Storage = "_Freight", DbType = "Money")]
//        public System.Nullable<decimal> Freight
//        {
//            get
//            {
//                return this._Freight;
//            }
//            set
//            {
//                if ((this._Freight != value))
//                {
//                    this._Freight = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipName", DbType = "NVarChar(40)")]
//        public string ShipName
//        {
//            get
//            {
//                return this._ShipName;
//            }
//            set
//            {
//                if ((this._ShipName != value))
//                {
//                    this._ShipName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipAddress", DbType = "NVarChar(60)")]
//        public string ShipAddress
//        {
//            get
//            {
//                return this._ShipAddress;
//            }
//            set
//            {
//                if ((this._ShipAddress != value))
//                {
//                    this._ShipAddress = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipCity", DbType = "NVarChar(15)")]
//        public string ShipCity
//        {
//            get
//            {
//                return this._ShipCity;
//            }
//            set
//            {
//                if ((this._ShipCity != value))
//                {
//                    this._ShipCity = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipRegion", DbType = "NVarChar(15)")]
//        public string ShipRegion
//        {
//            get
//            {
//                return this._ShipRegion;
//            }
//            set
//            {
//                if ((this._ShipRegion != value))
//                {
//                    this._ShipRegion = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipPostalCode", DbType = "NVarChar(10)")]
//        public string ShipPostalCode
//        {
//            get
//            {
//                return this._ShipPostalCode;
//            }
//            set
//            {
//                if ((this._ShipPostalCode != value))
//                {
//                    this._ShipPostalCode = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipCountry", DbType = "NVarChar(15)")]
//        public string ShipCountry
//        {
//            get
//            {
//                return this._ShipCountry;
//            }
//            set
//            {
//                if ((this._ShipCountry != value))
//                {
//                    this._ShipCountry = value;
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this._CompanyName = value;
//                }
//            }
//        }

//        [Column(Storage = "_Address", DbType = "NVarChar(60)")]
//        public string Address
//        {
//            get
//            {
//                return this._Address;
//            }
//            set
//            {
//                if ((this._Address != value))
//                {
//                    this._Address = value;
//                }
//            }
//        }

//        [Column(Storage = "_City", DbType = "NVarChar(15)")]
//        public string City
//        {
//            get
//            {
//                return this._City;
//            }
//            set
//            {
//                if ((this._City != value))
//                {
//                    this._City = value;
//                }
//            }
//        }

//        [Column(Storage = "_Region", DbType = "NVarChar(15)")]
//        public string Region
//        {
//            get
//            {
//                return this._Region;
//            }
//            set
//            {
//                if ((this._Region != value))
//                {
//                    this._Region = value;
//                }
//            }
//        }

//        [Column(Storage = "_PostalCode", DbType = "NVarChar(10)")]
//        public string PostalCode
//        {
//            get
//            {
//                return this._PostalCode;
//            }
//            set
//            {
//                if ((this._PostalCode != value))
//                {
//                    this._PostalCode = value;
//                }
//            }
//        }

//        [Column(Storage = "_Country", DbType = "NVarChar(15)")]
//        public string Country
//        {
//            get
//            {
//                return this._Country;
//            }
//            set
//            {
//                if ((this._Country != value))
//                {
//                    this._Country = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Product Sales for 1997")]
//    public partial class ProductSalesFor1997
//    {

//        private string _CategoryName;

//        private string _ProductName;

//        private System.Nullable<decimal> _ProductSales;

//        public ProductSalesFor1997()
//        {
//        }

//        [Column(Storage = "_CategoryName", DbType = "NVarChar(15) NOT NULL", CanBeNull = false)]
//        public string CategoryName
//        {
//            get
//            {
//                return this._CategoryName;
//            }
//            set
//            {
//                if ((this._CategoryName != value))
//                {
//                    this._CategoryName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this._ProductName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ProductSales", DbType = "Money")]
//        public System.Nullable<decimal> ProductSales
//        {
//            get
//            {
//                return this._ProductSales;
//            }
//            set
//            {
//                if ((this._ProductSales != value))
//                {
//                    this._ProductSales = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Products")]
//    public partial class Product : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _ProductID;

//        private string _ProductName;

//        private System.Nullable<int> _SupplierID;

//        private System.Nullable<int> _CategoryID;

//        private string _QuantityPerUnit;

//        private System.Nullable<decimal> _UnitPrice;

//        private System.Nullable<short> _UnitsInStock;

//        private System.Nullable<short> _UnitsOnOrder;

//        private System.Nullable<short> _ReorderLevel;

//        private bool _Discontinued;

//        private EntitySet<OrderDetail> _OrderDetails;

//        private EntityRef<Category> _Category;

//        private EntityRef<Supplier> _Supplier;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnProductIDChanging(int value);
//        partial void OnProductIDChanged();
//        partial void OnProductNameChanging(string value);
//        partial void OnProductNameChanged();
//        partial void OnSupplierIDChanging(System.Nullable<int> value);
//        partial void OnSupplierIDChanged();
//        partial void OnCategoryIDChanging(System.Nullable<int> value);
//        partial void OnCategoryIDChanged();
//        partial void OnQuantityPerUnitChanging(string value);
//        partial void OnQuantityPerUnitChanged();
//        partial void OnUnitPriceChanging(System.Nullable<decimal> value);
//        partial void OnUnitPriceChanged();
//        partial void OnUnitsInStockChanging(System.Nullable<short> value);
//        partial void OnUnitsInStockChanged();
//        partial void OnUnitsOnOrderChanging(System.Nullable<short> value);
//        partial void OnUnitsOnOrderChanged();
//        partial void OnReorderLevelChanging(System.Nullable<short> value);
//        partial void OnReorderLevelChanged();
//        partial void OnDiscontinuedChanging(bool value);
//        partial void OnDiscontinuedChanged();
//        #endregion

//        public Product()
//        {
//            this._OrderDetails = new EntitySet<OrderDetail>(new Action<OrderDetail>(this.attach_OrderDetails), new Action<OrderDetail>(this.detach_OrderDetails));
//            this._Category = default(EntityRef<Category>);
//            this._Supplier = default(EntityRef<Supplier>);
//            OnCreated();
//        }

//        [Column(Storage = "_ProductID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
//        public int ProductID
//        {
//            get
//            {
//                return this._ProductID;
//            }
//            set
//            {
//                if ((this._ProductID != value))
//                {
//                    this.OnProductIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._ProductID = value;
//                    this.SendPropertyChanged("ProductID");
//                    this.OnProductIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this.OnProductNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._ProductName = value;
//                    this.SendPropertyChanged("ProductName");
//                    this.OnProductNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_SupplierID", DbType = "Int")]
//        public System.Nullable<int> SupplierID
//        {
//            get
//            {
//                return this._SupplierID;
//            }
//            set
//            {
//                if ((this._SupplierID != value))
//                {
//                    if (this._Supplier.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnSupplierIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._SupplierID = value;
//                    this.SendPropertyChanged("SupplierID");
//                    this.OnSupplierIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_CategoryID", DbType = "Int")]
//        public System.Nullable<int> CategoryID
//        {
//            get
//            {
//                return this._CategoryID;
//            }
//            set
//            {
//                if ((this._CategoryID != value))
//                {
//                    if (this._Category.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnCategoryIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._CategoryID = value;
//                    this.SendPropertyChanged("CategoryID");
//                    this.OnCategoryIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_QuantityPerUnit", DbType = "NVarChar(20)")]
//        public string QuantityPerUnit
//        {
//            get
//            {
//                return this._QuantityPerUnit;
//            }
//            set
//            {
//                if ((this._QuantityPerUnit != value))
//                {
//                    this.OnQuantityPerUnitChanging(value);
//                    this.SendPropertyChanging();
//                    this._QuantityPerUnit = value;
//                    this.SendPropertyChanged("QuantityPerUnit");
//                    this.OnQuantityPerUnitChanged();
//                }
//            }
//        }

//        [Column(Storage = "_UnitPrice", DbType = "Money")]
//        public System.Nullable<decimal> UnitPrice
//        {
//            get
//            {
//                return this._UnitPrice;
//            }
//            set
//            {
//                if ((this._UnitPrice != value))
//                {
//                    this.OnUnitPriceChanging(value);
//                    this.SendPropertyChanging();
//                    this._UnitPrice = value;
//                    this.SendPropertyChanged("UnitPrice");
//                    this.OnUnitPriceChanged();
//                }
//            }
//        }

//        [Column(Storage = "_UnitsInStock", DbType = "SmallInt")]
//        public System.Nullable<short> UnitsInStock
//        {
//            get
//            {
//                return this._UnitsInStock;
//            }
//            set
//            {
//                if ((this._UnitsInStock != value))
//                {
//                    this.OnUnitsInStockChanging(value);
//                    this.SendPropertyChanging();
//                    this._UnitsInStock = value;
//                    this.SendPropertyChanged("UnitsInStock");
//                    this.OnUnitsInStockChanged();
//                }
//            }
//        }

//        [Column(Storage = "_UnitsOnOrder", DbType = "SmallInt")]
//        public System.Nullable<short> UnitsOnOrder
//        {
//            get
//            {
//                return this._UnitsOnOrder;
//            }
//            set
//            {
//                if ((this._UnitsOnOrder != value))
//                {
//                    this.OnUnitsOnOrderChanging(value);
//                    this.SendPropertyChanging();
//                    this._UnitsOnOrder = value;
//                    this.SendPropertyChanged("UnitsOnOrder");
//                    this.OnUnitsOnOrderChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ReorderLevel", DbType = "SmallInt")]
//        public System.Nullable<short> ReorderLevel
//        {
//            get
//            {
//                return this._ReorderLevel;
//            }
//            set
//            {
//                if ((this._ReorderLevel != value))
//                {
//                    this.OnReorderLevelChanging(value);
//                    this.SendPropertyChanging();
//                    this._ReorderLevel = value;
//                    this.SendPropertyChanged("ReorderLevel");
//                    this.OnReorderLevelChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Discontinued", DbType = "Bit NOT NULL")]
//        public bool Discontinued
//        {
//            get
//            {
//                return this._Discontinued;
//            }
//            set
//            {
//                if ((this._Discontinued != value))
//                {
//                    this.OnDiscontinuedChanging(value);
//                    this.SendPropertyChanging();
//                    this._Discontinued = value;
//                    this.SendPropertyChanged("Discontinued");
//                    this.OnDiscontinuedChanged();
//                }
//            }
//        }

//        [Association(Name = "Product_OrderDetail", Storage = "_OrderDetails", OtherKey = "ProductID")]
//        public EntitySet<OrderDetail> OrderDetails
//        {
//            get
//            {
//                return this._OrderDetails;
//            }
//            set
//            {
//                this._OrderDetails.Assign(value);
//            }
//        }

//        [Association(Name = "Category_Product", Storage = "_Category", ThisKey = "CategoryID", IsForeignKey = true)]
//        public Category Category
//        {
//            get
//            {
//                return this._Category.Entity;
//            }
//            set
//            {
//                Category previousValue = this._Category.Entity;
//                if (((previousValue != value)
//                            || (this._Category.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._Category.Entity = null;
//                        previousValue.Products.Remove(this);
//                    }
//                    this._Category.Entity = value;
//                    if ((value != null))
//                    {
//                        value.Products.Add(this);
//                        this._CategoryID = value.CategoryID;
//                    }
//                    else
//                    {
//                        this._CategoryID = default(Nullable<int>);
//                    }
//                    this.SendPropertyChanged("Category");
//                }
//            }
//        }

//        [Association(Name = "Supplier_Product", Storage = "_Supplier", ThisKey = "SupplierID", IsForeignKey = true)]
//        public Supplier Supplier
//        {
//            get
//            {
//                return this._Supplier.Entity;
//            }
//            set
//            {
//                Supplier previousValue = this._Supplier.Entity;
//                if (((previousValue != value)
//                            || (this._Supplier.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._Supplier.Entity = null;
//                        previousValue.Products.Remove(this);
//                    }
//                    this._Supplier.Entity = value;
//                    if ((value != null))
//                    {
//                        value.Products.Add(this);
//                        this._SupplierID = value.SupplierID;
//                    }
//                    else
//                    {
//                        this._SupplierID = default(Nullable<int>);
//                    }
//                    this.SendPropertyChanged("Supplier");
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        private void attach_OrderDetails(OrderDetail entity)
//        {
//            this.SendPropertyChanging();
//            entity.Product = this;
//            this.SendPropertyChanged("OrderDetails");
//        }

//        private void detach_OrderDetails(OrderDetail entity)
//        {
//            this.SendPropertyChanging();
//            entity.Product = null;
//            this.SendPropertyChanged("OrderDetails");
//        }
//    }

//    [Table(Name = "dbo.Products Above Average Price")]
//    public partial class ProductsAboveAveragePrice
//    {

//        private string _ProductName;

//        private System.Nullable<decimal> _UnitPrice;

//        public ProductsAboveAveragePrice()
//        {
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this._ProductName = value;
//                }
//            }
//        }

//        [Column(Storage = "_UnitPrice", DbType = "Money")]
//        public System.Nullable<decimal> UnitPrice
//        {
//            get
//            {
//                return this._UnitPrice;
//            }
//            set
//            {
//                if ((this._UnitPrice != value))
//                {
//                    this._UnitPrice = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Products by Category")]
//    public partial class ProductsByCategory
//    {

//        private string _CategoryName;

//        private string _ProductName;

//        private string _QuantityPerUnit;

//        private System.Nullable<short> _UnitsInStock;

//        private bool _Discontinued;

//        public ProductsByCategory()
//        {
//        }

//        [Column(Storage = "_CategoryName", DbType = "NVarChar(15) NOT NULL", CanBeNull = false)]
//        public string CategoryName
//        {
//            get
//            {
//                return this._CategoryName;
//            }
//            set
//            {
//                if ((this._CategoryName != value))
//                {
//                    this._CategoryName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this._ProductName = value;
//                }
//            }
//        }

//        [Column(Storage = "_QuantityPerUnit", DbType = "NVarChar(20)")]
//        public string QuantityPerUnit
//        {
//            get
//            {
//                return this._QuantityPerUnit;
//            }
//            set
//            {
//                if ((this._QuantityPerUnit != value))
//                {
//                    this._QuantityPerUnit = value;
//                }
//            }
//        }

//        [Column(Storage = "_UnitsInStock", DbType = "SmallInt")]
//        public System.Nullable<short> UnitsInStock
//        {
//            get
//            {
//                return this._UnitsInStock;
//            }
//            set
//            {
//                if ((this._UnitsInStock != value))
//                {
//                    this._UnitsInStock = value;
//                }
//            }
//        }

//        [Column(Storage = "_Discontinued", DbType = "Bit NOT NULL")]
//        public bool Discontinued
//        {
//            get
//            {
//                return this._Discontinued;
//            }
//            set
//            {
//                if ((this._Discontinued != value))
//                {
//                    this._Discontinued = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Quarterly Orders")]
//    public partial class QuarterlyOrder
//    {

//        private string _CustomerID;

//        private string _CompanyName;

//        private string _City;

//        private string _Country;

//        public QuarterlyOrder()
//        {
//        }

//        [Column(Storage = "_CustomerID", DbType = "NChar(5)")]
//        public string CustomerID
//        {
//            get
//            {
//                return this._CustomerID;
//            }
//            set
//            {
//                if ((this._CustomerID != value))
//                {
//                    this._CustomerID = value;
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40)")]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this._CompanyName = value;
//                }
//            }
//        }

//        [Column(Storage = "_City", DbType = "NVarChar(15)")]
//        public string City
//        {
//            get
//            {
//                return this._City;
//            }
//            set
//            {
//                if ((this._City != value))
//                {
//                    this._City = value;
//                }
//            }
//        }

//        [Column(Storage = "_Country", DbType = "NVarChar(15)")]
//        public string Country
//        {
//            get
//            {
//                return this._Country;
//            }
//            set
//            {
//                if ((this._Country != value))
//                {
//                    this._Country = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Region")]
//    public partial class Region : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _RegionID;

//        private string _RegionDescription;

//        private EntitySet<Territory> _Territories;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnRegionIDChanging(int value);
//        partial void OnRegionIDChanged();
//        partial void OnRegionDescriptionChanging(string value);
//        partial void OnRegionDescriptionChanged();
//        #endregion

//        public Region()
//        {
//            this._Territories = new EntitySet<Territory>(new Action<Territory>(this.attach_Territories), new Action<Territory>(this.detach_Territories));
//            OnCreated();
//        }

//        [Column(Storage = "_RegionID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
//        public int RegionID
//        {
//            get
//            {
//                return this._RegionID;
//            }
//            set
//            {
//                if ((this._RegionID != value))
//                {
//                    this.OnRegionIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._RegionID = value;
//                    this.SendPropertyChanged("RegionID");
//                    this.OnRegionIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_RegionDescription", DbType = "NChar(50) NOT NULL", CanBeNull = false)]
//        public string RegionDescription
//        {
//            get
//            {
//                return this._RegionDescription;
//            }
//            set
//            {
//                if ((this._RegionDescription != value))
//                {
//                    this.OnRegionDescriptionChanging(value);
//                    this.SendPropertyChanging();
//                    this._RegionDescription = value;
//                    this.SendPropertyChanged("RegionDescription");
//                    this.OnRegionDescriptionChanged();
//                }
//            }
//        }

//        [Association(Name = "Region_Territory", Storage = "_Territories", OtherKey = "RegionID")]
//        public EntitySet<Territory> Territories
//        {
//            get
//            {
//                return this._Territories;
//            }
//            set
//            {
//                this._Territories.Assign(value);
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        private void attach_Territories(Territory entity)
//        {
//            this.SendPropertyChanging();
//            entity.Region = this;
//            this.SendPropertyChanged("Territories");
//        }

//        private void detach_Territories(Territory entity)
//        {
//            this.SendPropertyChanging();
//            entity.Region = null;
//            this.SendPropertyChanged("Territories");
//        }
//    }

//    [Table(Name = "dbo.Sales by Category")]
//    public partial class SalesByCategory
//    {

//        private int _CategoryID;

//        private string _CategoryName;

//        private string _ProductName;

//        private System.Nullable<decimal> _ProductSales;

//        public SalesByCategory()
//        {
//        }

//        [Column(Storage = "_CategoryID", DbType = "Int NOT NULL IDENTITY")]
//        public int CategoryID
//        {
//            get
//            {
//                return this._CategoryID;
//            }
//            set
//            {
//                if ((this._CategoryID != value))
//                {
//                    this._CategoryID = value;
//                }
//            }
//        }

//        [Column(Storage = "_CategoryName", DbType = "NVarChar(15) NOT NULL", CanBeNull = false)]
//        public string CategoryName
//        {
//            get
//            {
//                return this._CategoryName;
//            }
//            set
//            {
//                if ((this._CategoryName != value))
//                {
//                    this._CategoryName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this._ProductName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ProductSales", DbType = "Money")]
//        public System.Nullable<decimal> ProductSales
//        {
//            get
//            {
//                return this._ProductSales;
//            }
//            set
//            {
//                if ((this._ProductSales != value))
//                {
//                    this._ProductSales = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Sales Totals by Amount")]
//    public partial class SalesTotalsByAmount
//    {

//        private System.Nullable<decimal> _SaleAmount;

//        private int _OrderID;

//        private string _CompanyName;

//        private System.Nullable<System.DateTime> _ShippedDate;

//        public SalesTotalsByAmount()
//        {
//        }

//        [Column(Storage = "_SaleAmount", DbType = "Money")]
//        public System.Nullable<decimal> SaleAmount
//        {
//            get
//            {
//                return this._SaleAmount;
//            }
//            set
//            {
//                if ((this._SaleAmount != value))
//                {
//                    this._SaleAmount = value;
//                }
//            }
//        }

//        [Column(Storage = "_OrderID", DbType = "Int NOT NULL IDENTITY")]
//        public int OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    this._OrderID = value;
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this._CompanyName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShippedDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> ShippedDate
//        {
//            get
//            {
//                return this._ShippedDate;
//            }
//            set
//            {
//                if ((this._ShippedDate != value))
//                {
//                    this._ShippedDate = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Shippers")]
//    public partial class Shipper : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _ShipperID;

//        private string _CompanyName;

//        private string _Phone;

//        private EntitySet<Order> _Orders;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnShipperIDChanging(int value);
//        partial void OnShipperIDChanged();
//        partial void OnCompanyNameChanging(string value);
//        partial void OnCompanyNameChanged();
//        partial void OnPhoneChanging(string value);
//        partial void OnPhoneChanged();
//        #endregion

//        public Shipper()
//        {
//            this._Orders = new EntitySet<Order>(new Action<Order>(this.attach_Orders), new Action<Order>(this.detach_Orders));
//            OnCreated();
//        }

//        [Column(Storage = "_ShipperID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
//        public int ShipperID
//        {
//            get
//            {
//                return this._ShipperID;
//            }
//            set
//            {
//                if ((this._ShipperID != value))
//                {
//                    this.OnShipperIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._ShipperID = value;
//                    this.SendPropertyChanged("ShipperID");
//                    this.OnShipperIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this.OnCompanyNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._CompanyName = value;
//                    this.SendPropertyChanged("CompanyName");
//                    this.OnCompanyNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Phone", DbType = "NVarChar(24)")]
//        public string Phone
//        {
//            get
//            {
//                return this._Phone;
//            }
//            set
//            {
//                if ((this._Phone != value))
//                {
//                    this.OnPhoneChanging(value);
//                    this.SendPropertyChanging();
//                    this._Phone = value;
//                    this.SendPropertyChanged("Phone");
//                    this.OnPhoneChanged();
//                }
//            }
//        }

//        [Association(Name = "Shipper_Order", Storage = "_Orders", OtherKey = "ShipVia")]
//        public EntitySet<Order> Orders
//        {
//            get
//            {
//                return this._Orders;
//            }
//            set
//            {
//                this._Orders.Assign(value);
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        private void attach_Orders(Order entity)
//        {
//            this.SendPropertyChanging();
//            entity.Shipper = this;
//            this.SendPropertyChanged("Orders");
//        }

//        private void detach_Orders(Order entity)
//        {
//            this.SendPropertyChanging();
//            entity.Shipper = null;
//            this.SendPropertyChanged("Orders");
//        }
//    }

//    [Table(Name = "dbo.Summary of Sales by Quarter")]
//    public partial class SummaryOfSalesByQuarter
//    {

//        private System.Nullable<System.DateTime> _ShippedDate;

//        private int _OrderID;

//        private System.Nullable<decimal> _Subtotal;

//        public SummaryOfSalesByQuarter()
//        {
//        }

//        [Column(Storage = "_ShippedDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> ShippedDate
//        {
//            get
//            {
//                return this._ShippedDate;
//            }
//            set
//            {
//                if ((this._ShippedDate != value))
//                {
//                    this._ShippedDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_OrderID", DbType = "Int NOT NULL IDENTITY")]
//        public int OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    this._OrderID = value;
//                }
//            }
//        }

//        [Column(Storage = "_Subtotal", DbType = "Money")]
//        public System.Nullable<decimal> Subtotal
//        {
//            get
//            {
//                return this._Subtotal;
//            }
//            set
//            {
//                if ((this._Subtotal != value))
//                {
//                    this._Subtotal = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Summary of Sales by Year")]
//    public partial class SummaryOfSalesByYear
//    {

//        private System.Nullable<System.DateTime> _ShippedDate;

//        private int _OrderID;

//        private System.Nullable<decimal> _Subtotal;

//        public SummaryOfSalesByYear()
//        {
//        }

//        [Column(Storage = "_ShippedDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> ShippedDate
//        {
//            get
//            {
//                return this._ShippedDate;
//            }
//            set
//            {
//                if ((this._ShippedDate != value))
//                {
//                    this._ShippedDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_OrderID", DbType = "Int NOT NULL IDENTITY")]
//        public int OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    this._OrderID = value;
//                }
//            }
//        }

//        [Column(Storage = "_Subtotal", DbType = "Money")]
//        public System.Nullable<decimal> Subtotal
//        {
//            get
//            {
//                return this._Subtotal;
//            }
//            set
//            {
//                if ((this._Subtotal != value))
//                {
//                    this._Subtotal = value;
//                }
//            }
//        }
//    }

//    [Table(Name = "dbo.Suppliers")]
//    public partial class Supplier : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _SupplierID;

//        private string _CompanyName;

//        private string _ContactName;

//        private string _ContactTitle;

//        private string _Address;

//        private string _City;

//        private string _Region;

//        private string _PostalCode;

//        private string _Country;

//        private string _Phone;

//        private string _Fax;

//        private string _HomePage;

//        private EntitySet<Product> _Products;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnSupplierIDChanging(int value);
//        partial void OnSupplierIDChanged();
//        partial void OnCompanyNameChanging(string value);
//        partial void OnCompanyNameChanged();
//        partial void OnContactNameChanging(string value);
//        partial void OnContactNameChanged();
//        partial void OnContactTitleChanging(string value);
//        partial void OnContactTitleChanged();
//        partial void OnAddressChanging(string value);
//        partial void OnAddressChanged();
//        partial void OnCityChanging(string value);
//        partial void OnCityChanged();
//        partial void OnRegionChanging(string value);
//        partial void OnRegionChanged();
//        partial void OnPostalCodeChanging(string value);
//        partial void OnPostalCodeChanged();
//        partial void OnCountryChanging(string value);
//        partial void OnCountryChanged();
//        partial void OnPhoneChanging(string value);
//        partial void OnPhoneChanged();
//        partial void OnFaxChanging(string value);
//        partial void OnFaxChanged();
//        partial void OnHomePageChanging(string value);
//        partial void OnHomePageChanged();
//        #endregion

//        public Supplier()
//        {
//            this._Products = new EntitySet<Product>(new Action<Product>(this.attach_Products), new Action<Product>(this.detach_Products));
//            OnCreated();
//        }

//        [Column(Storage = "_SupplierID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
//        public int SupplierID
//        {
//            get
//            {
//                return this._SupplierID;
//            }
//            set
//            {
//                if ((this._SupplierID != value))
//                {
//                    this.OnSupplierIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._SupplierID = value;
//                    this.SendPropertyChanged("SupplierID");
//                    this.OnSupplierIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40) NOT NULL", CanBeNull = false)]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this.OnCompanyNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._CompanyName = value;
//                    this.SendPropertyChanged("CompanyName");
//                    this.OnCompanyNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ContactName", DbType = "NVarChar(30)")]
//        public string ContactName
//        {
//            get
//            {
//                return this._ContactName;
//            }
//            set
//            {
//                if ((this._ContactName != value))
//                {
//                    this.OnContactNameChanging(value);
//                    this.SendPropertyChanging();
//                    this._ContactName = value;
//                    this.SendPropertyChanged("ContactName");
//                    this.OnContactNameChanged();
//                }
//            }
//        }

//        [Column(Storage = "_ContactTitle", DbType = "NVarChar(30)")]
//        public string ContactTitle
//        {
//            get
//            {
//                return this._ContactTitle;
//            }
//            set
//            {
//                if ((this._ContactTitle != value))
//                {
//                    this.OnContactTitleChanging(value);
//                    this.SendPropertyChanging();
//                    this._ContactTitle = value;
//                    this.SendPropertyChanged("ContactTitle");
//                    this.OnContactTitleChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Address", DbType = "NVarChar(60)")]
//        public string Address
//        {
//            get
//            {
//                return this._Address;
//            }
//            set
//            {
//                if ((this._Address != value))
//                {
//                    this.OnAddressChanging(value);
//                    this.SendPropertyChanging();
//                    this._Address = value;
//                    this.SendPropertyChanged("Address");
//                    this.OnAddressChanged();
//                }
//            }
//        }

//        [Column(Storage = "_City", DbType = "NVarChar(15)")]
//        public string City
//        {
//            get
//            {
//                return this._City;
//            }
//            set
//            {
//                if ((this._City != value))
//                {
//                    this.OnCityChanging(value);
//                    this.SendPropertyChanging();
//                    this._City = value;
//                    this.SendPropertyChanged("City");
//                    this.OnCityChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Region", DbType = "NVarChar(15)")]
//        public string Region
//        {
//            get
//            {
//                return this._Region;
//            }
//            set
//            {
//                if ((this._Region != value))
//                {
//                    this.OnRegionChanging(value);
//                    this.SendPropertyChanging();
//                    this._Region = value;
//                    this.SendPropertyChanged("Region");
//                    this.OnRegionChanged();
//                }
//            }
//        }

//        [Column(Storage = "_PostalCode", DbType = "NVarChar(10)")]
//        public string PostalCode
//        {
//            get
//            {
//                return this._PostalCode;
//            }
//            set
//            {
//                if ((this._PostalCode != value))
//                {
//                    this.OnPostalCodeChanging(value);
//                    this.SendPropertyChanging();
//                    this._PostalCode = value;
//                    this.SendPropertyChanged("PostalCode");
//                    this.OnPostalCodeChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Country", DbType = "NVarChar(15)")]
//        public string Country
//        {
//            get
//            {
//                return this._Country;
//            }
//            set
//            {
//                if ((this._Country != value))
//                {
//                    this.OnCountryChanging(value);
//                    this.SendPropertyChanging();
//                    this._Country = value;
//                    this.SendPropertyChanged("Country");
//                    this.OnCountryChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Phone", DbType = "NVarChar(24)")]
//        public string Phone
//        {
//            get
//            {
//                return this._Phone;
//            }
//            set
//            {
//                if ((this._Phone != value))
//                {
//                    this.OnPhoneChanging(value);
//                    this.SendPropertyChanging();
//                    this._Phone = value;
//                    this.SendPropertyChanged("Phone");
//                    this.OnPhoneChanged();
//                }
//            }
//        }

//        [Column(Storage = "_Fax", DbType = "NVarChar(24)")]
//        public string Fax
//        {
//            get
//            {
//                return this._Fax;
//            }
//            set
//            {
//                if ((this._Fax != value))
//                {
//                    this.OnFaxChanging(value);
//                    this.SendPropertyChanging();
//                    this._Fax = value;
//                    this.SendPropertyChanged("Fax");
//                    this.OnFaxChanged();
//                }
//            }
//        }

//        [Column(Storage = "_HomePage", DbType = "NText", UpdateCheck = UpdateCheck.Never)]
//        public string HomePage
//        {
//            get
//            {
//                return this._HomePage;
//            }
//            set
//            {
//                if ((this._HomePage != value))
//                {
//                    this.OnHomePageChanging(value);
//                    this.SendPropertyChanging();
//                    this._HomePage = value;
//                    this.SendPropertyChanged("HomePage");
//                    this.OnHomePageChanged();
//                }
//            }
//        }

//        [Association(Name = "Supplier_Product", Storage = "_Products", OtherKey = "SupplierID")]
//        public EntitySet<Product> Products
//        {
//            get
//            {
//                return this._Products;
//            }
//            set
//            {
//                this._Products.Assign(value);
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        private void attach_Products(Product entity)
//        {
//            this.SendPropertyChanging();
//            entity.Supplier = this;
//            this.SendPropertyChanged("Products");
//        }

//        private void detach_Products(Product entity)
//        {
//            this.SendPropertyChanging();
//            entity.Supplier = null;
//            this.SendPropertyChanged("Products");
//        }
//    }

//    [Table(Name = "dbo.Territories")]
//    public partial class Territory : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private string _TerritoryID;

//        private string _TerritoryDescription;

//        private int _RegionID;

//        private EntitySet<EmployeeTerritory> _EmployeeTerritories;

//        private EntityRef<Region> _Region;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnTerritoryIDChanging(string value);
//        partial void OnTerritoryIDChanged();
//        partial void OnTerritoryDescriptionChanging(string value);
//        partial void OnTerritoryDescriptionChanged();
//        partial void OnRegionIDChanging(int value);
//        partial void OnRegionIDChanged();
//        #endregion

//        public Territory()
//        {
//            this._EmployeeTerritories = new EntitySet<EmployeeTerritory>(new Action<EmployeeTerritory>(this.attach_EmployeeTerritories), new Action<EmployeeTerritory>(this.detach_EmployeeTerritories));
//            this._Region = default(EntityRef<Region>);
//            OnCreated();
//        }

//        [Column(Storage = "_TerritoryID", DbType = "NVarChar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
//        public string TerritoryID
//        {
//            get
//            {
//                return this._TerritoryID;
//            }
//            set
//            {
//                if ((this._TerritoryID != value))
//                {
//                    this.OnTerritoryIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._TerritoryID = value;
//                    this.SendPropertyChanged("TerritoryID");
//                    this.OnTerritoryIDChanged();
//                }
//            }
//        }

//        [Column(Storage = "_TerritoryDescription", DbType = "NChar(50) NOT NULL", CanBeNull = false)]
//        public string TerritoryDescription
//        {
//            get
//            {
//                return this._TerritoryDescription;
//            }
//            set
//            {
//                if ((this._TerritoryDescription != value))
//                {
//                    this.OnTerritoryDescriptionChanging(value);
//                    this.SendPropertyChanging();
//                    this._TerritoryDescription = value;
//                    this.SendPropertyChanged("TerritoryDescription");
//                    this.OnTerritoryDescriptionChanged();
//                }
//            }
//        }

//        [Column(Storage = "_RegionID", DbType = "Int NOT NULL")]
//        public int RegionID
//        {
//            get
//            {
//                return this._RegionID;
//            }
//            set
//            {
//                if ((this._RegionID != value))
//                {
//                    if (this._Region.HasLoadedOrAssignedValue)
//                    {
//                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
//                    }
//                    this.OnRegionIDChanging(value);
//                    this.SendPropertyChanging();
//                    this._RegionID = value;
//                    this.SendPropertyChanged("RegionID");
//                    this.OnRegionIDChanged();
//                }
//            }
//        }

//        [Association(Name = "Territory_EmployeeTerritory", Storage = "_EmployeeTerritories", OtherKey = "TerritoryID")]
//        public EntitySet<EmployeeTerritory> EmployeeTerritories
//        {
//            get
//            {
//                return this._EmployeeTerritories;
//            }
//            set
//            {
//                this._EmployeeTerritories.Assign(value);
//            }
//        }

//        [Association(Name = "Region_Territory", Storage = "_Region", ThisKey = "RegionID", IsForeignKey = true)]
//        public Region Region
//        {
//            get
//            {
//                return this._Region.Entity;
//            }
//            set
//            {
//                Region previousValue = this._Region.Entity;
//                if (((previousValue != value)
//                            || (this._Region.HasLoadedOrAssignedValue == false)))
//                {
//                    this.SendPropertyChanging();
//                    if ((previousValue != null))
//                    {
//                        this._Region.Entity = null;
//                        previousValue.Territories.Remove(this);
//                    }
//                    this._Region.Entity = value;
//                    if ((value != null))
//                    {
//                        value.Territories.Add(this);
//                        this._RegionID = value.RegionID;
//                    }
//                    else
//                    {
//                        this._RegionID = default(int);
//                    }
//                    this.SendPropertyChanged("Region");
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        private void attach_EmployeeTerritories(EmployeeTerritory entity)
//        {
//            this.SendPropertyChanging();
//            entity.Territory = this;
//            this.SendPropertyChanged("EmployeeTerritories");
//        }

//        private void detach_EmployeeTerritories(EmployeeTerritory entity)
//        {
//            this.SendPropertyChanging();
//            entity.Territory = null;
//            this.SendPropertyChanged("EmployeeTerritories");
//        }
//    }

//    public partial class CustomersByCityResult
//    {

//        private string _CustomerID;

//        private string _ContactName;

//        private string _CompanyName;

//        private string _City;

//        public CustomersByCityResult()
//        {
//        }

//        [Column(Storage = "_CustomerID", DbType = "NChar(5)")]
//        public string CustomerID
//        {
//            get
//            {
//                return this._CustomerID;
//            }
//            set
//            {
//                if ((this._CustomerID != value))
//                {
//                    this._CustomerID = value;
//                }
//            }
//        }

//        [Column(Storage = "_ContactName", DbType = "NVarChar(30)")]
//        public string ContactName
//        {
//            get
//            {
//                return this._ContactName;
//            }
//            set
//            {
//                if ((this._ContactName != value))
//                {
//                    this._ContactName = value;
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40)")]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this._CompanyName = value;
//                }
//            }
//        }

//        [Column(Storage = "_City", DbType = "NVarChar(15)")]
//        public string City
//        {
//            get
//            {
//                return this._City;
//            }
//            set
//            {
//                if ((this._City != value))
//                {
//                    this._City = value;
//                }
//            }
//        }
//    }

//    public partial class CustOrderHistResult
//    {

//        private string _ProductName;

//        private System.Nullable<int> _Total;

//        public CustOrderHistResult()
//        {
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40)")]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this._ProductName = value;
//                }
//            }
//        }

//        [Column(Storage = "_Total", DbType = "Int")]
//        public System.Nullable<int> Total
//        {
//            get
//            {
//                return this._Total;
//            }
//            set
//            {
//                if ((this._Total != value))
//                {
//                    this._Total = value;
//                }
//            }
//        }
//    }

//    public partial class CustOrdersDetailResult
//    {

//        private string _ProductName;

//        private System.Nullable<decimal> _UnitPrice;

//        private System.Nullable<short> _Quantity;

//        private System.Nullable<int> _Discount;

//        private System.Nullable<decimal> _ExtendedPrice;

//        public CustOrdersDetailResult()
//        {
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40)")]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this._ProductName = value;
//                }
//            }
//        }

//        [Column(Storage = "_UnitPrice", DbType = "Money")]
//        public System.Nullable<decimal> UnitPrice
//        {
//            get
//            {
//                return this._UnitPrice;
//            }
//            set
//            {
//                if ((this._UnitPrice != value))
//                {
//                    this._UnitPrice = value;
//                }
//            }
//        }

//        [Column(Storage = "_Quantity", DbType = "SmallInt")]
//        public System.Nullable<short> Quantity
//        {
//            get
//            {
//                return this._Quantity;
//            }
//            set
//            {
//                if ((this._Quantity != value))
//                {
//                    this._Quantity = value;
//                }
//            }
//        }

//        [Column(Storage = "_Discount", DbType = "Int")]
//        public System.Nullable<int> Discount
//        {
//            get
//            {
//                return this._Discount;
//            }
//            set
//            {
//                if ((this._Discount != value))
//                {
//                    this._Discount = value;
//                }
//            }
//        }

//        [Column(Storage = "_ExtendedPrice", DbType = "Money")]
//        public System.Nullable<decimal> ExtendedPrice
//        {
//            get
//            {
//                return this._ExtendedPrice;
//            }
//            set
//            {
//                if ((this._ExtendedPrice != value))
//                {
//                    this._ExtendedPrice = value;
//                }
//            }
//        }
//    }

//    public partial class CustOrdersOrdersResult
//    {

//        private System.Nullable<int> _OrderID;

//        private System.Nullable<System.DateTime> _OrderDate;

//        private System.Nullable<System.DateTime> _RequiredDate;

//        private System.Nullable<System.DateTime> _ShippedDate;

//        public CustOrdersOrdersResult()
//        {
//        }

//        [Column(Storage = "_OrderID", DbType = "Int")]
//        public System.Nullable<int> OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    this._OrderID = value;
//                }
//            }
//        }

//        [Column(Storage = "_OrderDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> OrderDate
//        {
//            get
//            {
//                return this._OrderDate;
//            }
//            set
//            {
//                if ((this._OrderDate != value))
//                {
//                    this._OrderDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_RequiredDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> RequiredDate
//        {
//            get
//            {
//                return this._RequiredDate;
//            }
//            set
//            {
//                if ((this._RequiredDate != value))
//                {
//                    this._RequiredDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShippedDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> ShippedDate
//        {
//            get
//            {
//                return this._ShippedDate;
//            }
//            set
//            {
//                if ((this._ShippedDate != value))
//                {
//                    this._ShippedDate = value;
//                }
//            }
//        }
//    }

//    public partial class EmployeeSalesByCountryResult
//    {

//        private string _Country;

//        private string _LastName;

//        private string _FirstName;

//        private System.Nullable<System.DateTime> _ShippedDate;

//        private System.Nullable<int> _OrderID;

//        private System.Nullable<decimal> _SaleAmount;

//        public EmployeeSalesByCountryResult()
//        {
//        }

//        [Column(Storage = "_Country", DbType = "NVarChar(15)")]
//        public string Country
//        {
//            get
//            {
//                return this._Country;
//            }
//            set
//            {
//                if ((this._Country != value))
//                {
//                    this._Country = value;
//                }
//            }
//        }

//        [Column(Storage = "_LastName", DbType = "NVarChar(20)")]
//        public string LastName
//        {
//            get
//            {
//                return this._LastName;
//            }
//            set
//            {
//                if ((this._LastName != value))
//                {
//                    this._LastName = value;
//                }
//            }
//        }

//        [Column(Storage = "_FirstName", DbType = "NVarChar(10)")]
//        public string FirstName
//        {
//            get
//            {
//                return this._FirstName;
//            }
//            set
//            {
//                if ((this._FirstName != value))
//                {
//                    this._FirstName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShippedDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> ShippedDate
//        {
//            get
//            {
//                return this._ShippedDate;
//            }
//            set
//            {
//                if ((this._ShippedDate != value))
//                {
//                    this._ShippedDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_OrderID", DbType = "Int")]
//        public System.Nullable<int> OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    this._OrderID = value;
//                }
//            }
//        }

//        [Column(Storage = "_SaleAmount", DbType = "Money")]
//        public System.Nullable<decimal> SaleAmount
//        {
//            get
//            {
//                return this._SaleAmount;
//            }
//            set
//            {
//                if ((this._SaleAmount != value))
//                {
//                    this._SaleAmount = value;
//                }
//            }
//        }
//    }

//    public partial class CustomerResultSet
//    {

//        private string _CustomerID;

//        private string _CompanyName;

//        private string _ContactName;

//        private string _ContactTitle;

//        private string _Address;

//        private string _City;

//        private string _Region;

//        private string _PostalCode;

//        private string _Country;

//        private string _Phone;

//        private string _Fax;

//        public CustomerResultSet()
//        {
//        }

//        [Column(Storage = "_CustomerID", DbType = "NChar(5)")]
//        public string CustomerID
//        {
//            get
//            {
//                return this._CustomerID;
//            }
//            set
//            {
//                if ((this._CustomerID != value))
//                {
//                    this._CustomerID = value;
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40)")]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this._CompanyName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ContactName", DbType = "NVarChar(30)")]
//        public string ContactName
//        {
//            get
//            {
//                return this._ContactName;
//            }
//            set
//            {
//                if ((this._ContactName != value))
//                {
//                    this._ContactName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ContactTitle", DbType = "NVarChar(30)")]
//        public string ContactTitle
//        {
//            get
//            {
//                return this._ContactTitle;
//            }
//            set
//            {
//                if ((this._ContactTitle != value))
//                {
//                    this._ContactTitle = value;
//                }
//            }
//        }

//        [Column(Storage = "_Address", DbType = "NVarChar(60)")]
//        public string Address
//        {
//            get
//            {
//                return this._Address;
//            }
//            set
//            {
//                if ((this._Address != value))
//                {
//                    this._Address = value;
//                }
//            }
//        }

//        [Column(Storage = "_City", DbType = "NVarChar(15)")]
//        public string City
//        {
//            get
//            {
//                return this._City;
//            }
//            set
//            {
//                if ((this._City != value))
//                {
//                    this._City = value;
//                }
//            }
//        }

//        [Column(Storage = "_Region", DbType = "NVarChar(15)")]
//        public string Region
//        {
//            get
//            {
//                return this._Region;
//            }
//            set
//            {
//                if ((this._Region != value))
//                {
//                    this._Region = value;
//                }
//            }
//        }

//        [Column(Storage = "_PostalCode", DbType = "NVarChar(10)")]
//        public string PostalCode
//        {
//            get
//            {
//                return this._PostalCode;
//            }
//            set
//            {
//                if ((this._PostalCode != value))
//                {
//                    this._PostalCode = value;
//                }
//            }
//        }

//        [Column(Storage = "_Country", DbType = "NVarChar(15)")]
//        public string Country
//        {
//            get
//            {
//                return this._Country;
//            }
//            set
//            {
//                if ((this._Country != value))
//                {
//                    this._Country = value;
//                }
//            }
//        }

//        [Column(Storage = "_Phone", DbType = "NVarChar(24)")]
//        public string Phone
//        {
//            get
//            {
//                return this._Phone;
//            }
//            set
//            {
//                if ((this._Phone != value))
//                {
//                    this._Phone = value;
//                }
//            }
//        }

//        [Column(Storage = "_Fax", DbType = "NVarChar(24)")]
//        public string Fax
//        {
//            get
//            {
//                return this._Fax;
//            }
//            set
//            {
//                if ((this._Fax != value))
//                {
//                    this._Fax = value;
//                }
//            }
//        }
//    }

//    public partial class OrdersResultSet
//    {

//        private System.Nullable<int> _OrderID;

//        private string _CustomerID;

//        private System.Nullable<int> _EmployeeID;

//        private System.Nullable<System.DateTime> _OrderDate;

//        private System.Nullable<System.DateTime> _RequiredDate;

//        private System.Nullable<System.DateTime> _ShippedDate;

//        private System.Nullable<int> _ShipVia;

//        private System.Nullable<decimal> _Freight;

//        private string _ShipName;

//        private string _ShipAddress;

//        private string _ShipCity;

//        private string _ShipRegion;

//        private string _ShipPostalCode;

//        private string _ShipCountry;

//        public OrdersResultSet()
//        {
//        }

//        [Column(Storage = "_OrderID", DbType = "Int")]
//        public System.Nullable<int> OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    this._OrderID = value;
//                }
//            }
//        }

//        [Column(Storage = "_CustomerID", DbType = "NChar(5)")]
//        public string CustomerID
//        {
//            get
//            {
//                return this._CustomerID;
//            }
//            set
//            {
//                if ((this._CustomerID != value))
//                {
//                    this._CustomerID = value;
//                }
//            }
//        }

//        [Column(Storage = "_EmployeeID", DbType = "Int")]
//        public System.Nullable<int> EmployeeID
//        {
//            get
//            {
//                return this._EmployeeID;
//            }
//            set
//            {
//                if ((this._EmployeeID != value))
//                {
//                    this._EmployeeID = value;
//                }
//            }
//        }

//        [Column(Storage = "_OrderDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> OrderDate
//        {
//            get
//            {
//                return this._OrderDate;
//            }
//            set
//            {
//                if ((this._OrderDate != value))
//                {
//                    this._OrderDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_RequiredDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> RequiredDate
//        {
//            get
//            {
//                return this._RequiredDate;
//            }
//            set
//            {
//                if ((this._RequiredDate != value))
//                {
//                    this._RequiredDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShippedDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> ShippedDate
//        {
//            get
//            {
//                return this._ShippedDate;
//            }
//            set
//            {
//                if ((this._ShippedDate != value))
//                {
//                    this._ShippedDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipVia", DbType = "Int")]
//        public System.Nullable<int> ShipVia
//        {
//            get
//            {
//                return this._ShipVia;
//            }
//            set
//            {
//                if ((this._ShipVia != value))
//                {
//                    this._ShipVia = value;
//                }
//            }
//        }

//        [Column(Storage = "_Freight", DbType = "Money")]
//        public System.Nullable<decimal> Freight
//        {
//            get
//            {
//                return this._Freight;
//            }
//            set
//            {
//                if ((this._Freight != value))
//                {
//                    this._Freight = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipName", DbType = "NVarChar(40)")]
//        public string ShipName
//        {
//            get
//            {
//                return this._ShipName;
//            }
//            set
//            {
//                if ((this._ShipName != value))
//                {
//                    this._ShipName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipAddress", DbType = "NVarChar(60)")]
//        public string ShipAddress
//        {
//            get
//            {
//                return this._ShipAddress;
//            }
//            set
//            {
//                if ((this._ShipAddress != value))
//                {
//                    this._ShipAddress = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipCity", DbType = "NVarChar(15)")]
//        public string ShipCity
//        {
//            get
//            {
//                return this._ShipCity;
//            }
//            set
//            {
//                if ((this._ShipCity != value))
//                {
//                    this._ShipCity = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipRegion", DbType = "NVarChar(15)")]
//        public string ShipRegion
//        {
//            get
//            {
//                return this._ShipRegion;
//            }
//            set
//            {
//                if ((this._ShipRegion != value))
//                {
//                    this._ShipRegion = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipPostalCode", DbType = "NVarChar(10)")]
//        public string ShipPostalCode
//        {
//            get
//            {
//                return this._ShipPostalCode;
//            }
//            set
//            {
//                if ((this._ShipPostalCode != value))
//                {
//                    this._ShipPostalCode = value;
//                }
//            }
//        }

//        [Column(Storage = "_ShipCountry", DbType = "NVarChar(15)")]
//        public string ShipCountry
//        {
//            get
//            {
//                return this._ShipCountry;
//            }
//            set
//            {
//                if ((this._ShipCountry != value))
//                {
//                    this._ShipCountry = value;
//                }
//            }
//        }
//    }

//    public partial class ProductsUnderThisUnitPriceResult
//    {

//        private System.Nullable<int> _ProductID;

//        private string _ProductName;

//        private System.Nullable<int> _SupplierID;

//        private System.Nullable<int> _CategoryID;

//        private string _QuantityPerUnit;

//        private System.Nullable<decimal> _UnitPrice;

//        private System.Nullable<short> _UnitsInStock;

//        private System.Nullable<short> _UnitsOnOrder;

//        private System.Nullable<short> _ReorderLevel;

//        private System.Nullable<bool> _Discontinued;

//        public ProductsUnderThisUnitPriceResult()
//        {
//        }

//        [Column(Storage = "_ProductID", DbType = "Int")]
//        public System.Nullable<int> ProductID
//        {
//            get
//            {
//                return this._ProductID;
//            }
//            set
//            {
//                if ((this._ProductID != value))
//                {
//                    this._ProductID = value;
//                }
//            }
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40)")]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this._ProductName = value;
//                }
//            }
//        }

//        [Column(Storage = "_SupplierID", DbType = "Int")]
//        public System.Nullable<int> SupplierID
//        {
//            get
//            {
//                return this._SupplierID;
//            }
//            set
//            {
//                if ((this._SupplierID != value))
//                {
//                    this._SupplierID = value;
//                }
//            }
//        }

//        [Column(Storage = "_CategoryID", DbType = "Int")]
//        public System.Nullable<int> CategoryID
//        {
//            get
//            {
//                return this._CategoryID;
//            }
//            set
//            {
//                if ((this._CategoryID != value))
//                {
//                    this._CategoryID = value;
//                }
//            }
//        }

//        [Column(Storage = "_QuantityPerUnit", DbType = "NVarChar(20)")]
//        public string QuantityPerUnit
//        {
//            get
//            {
//                return this._QuantityPerUnit;
//            }
//            set
//            {
//                if ((this._QuantityPerUnit != value))
//                {
//                    this._QuantityPerUnit = value;
//                }
//            }
//        }

//        [Column(Storage = "_UnitPrice", DbType = "Money")]
//        public System.Nullable<decimal> UnitPrice
//        {
//            get
//            {
//                return this._UnitPrice;
//            }
//            set
//            {
//                if ((this._UnitPrice != value))
//                {
//                    this._UnitPrice = value;
//                }
//            }
//        }

//        [Column(Storage = "_UnitsInStock", DbType = "SmallInt")]
//        public System.Nullable<short> UnitsInStock
//        {
//            get
//            {
//                return this._UnitsInStock;
//            }
//            set
//            {
//                if ((this._UnitsInStock != value))
//                {
//                    this._UnitsInStock = value;
//                }
//            }
//        }

//        [Column(Storage = "_UnitsOnOrder", DbType = "SmallInt")]
//        public System.Nullable<short> UnitsOnOrder
//        {
//            get
//            {
//                return this._UnitsOnOrder;
//            }
//            set
//            {
//                if ((this._UnitsOnOrder != value))
//                {
//                    this._UnitsOnOrder = value;
//                }
//            }
//        }

//        [Column(Storage = "_ReorderLevel", DbType = "SmallInt")]
//        public System.Nullable<short> ReorderLevel
//        {
//            get
//            {
//                return this._ReorderLevel;
//            }
//            set
//            {
//                if ((this._ReorderLevel != value))
//                {
//                    this._ReorderLevel = value;
//                }
//            }
//        }

//        [Column(Storage = "_Discontinued", DbType = "Bit")]
//        public System.Nullable<bool> Discontinued
//        {
//            get
//            {
//                return this._Discontinued;
//            }
//            set
//            {
//                if ((this._Discontinued != value))
//                {
//                    this._Discontinued = value;
//                }
//            }
//        }
//    }

//    public partial class SalesByYearResult
//    {

//        private System.Nullable<System.DateTime> _ShippedDate;

//        private System.Nullable<int> _OrderID;

//        private System.Nullable<decimal> _Subtotal;

//        private string _Year;

//        public SalesByYearResult()
//        {
//        }

//        [Column(Storage = "_ShippedDate", DbType = "DateTime")]
//        public System.Nullable<System.DateTime> ShippedDate
//        {
//            get
//            {
//                return this._ShippedDate;
//            }
//            set
//            {
//                if ((this._ShippedDate != value))
//                {
//                    this._ShippedDate = value;
//                }
//            }
//        }

//        [Column(Storage = "_OrderID", DbType = "Int")]
//        public System.Nullable<int> OrderID
//        {
//            get
//            {
//                return this._OrderID;
//            }
//            set
//            {
//                if ((this._OrderID != value))
//                {
//                    this._OrderID = value;
//                }
//            }
//        }

//        [Column(Storage = "_Subtotal", DbType = "Money")]
//        public System.Nullable<decimal> Subtotal
//        {
//            get
//            {
//                return this._Subtotal;
//            }
//            set
//            {
//                if ((this._Subtotal != value))
//                {
//                    this._Subtotal = value;
//                }
//            }
//        }

//        [Column(Storage = "_Year", DbType = "NVarChar(30)")]
//        public string Year
//        {
//            get
//            {
//                return this._Year;
//            }
//            set
//            {
//                if ((this._Year != value))
//                {
//                    this._Year = value;
//                }
//            }
//        }
//    }

//    public partial class SalesByCategoryResult
//    {

//        private string _ProductName;

//        private System.Nullable<decimal> _TotalPurchase;

//        public SalesByCategoryResult()
//        {
//        }

//        [Column(Storage = "_ProductName", DbType = "NVarChar(40)")]
//        public string ProductName
//        {
//            get
//            {
//                return this._ProductName;
//            }
//            set
//            {
//                if ((this._ProductName != value))
//                {
//                    this._ProductName = value;
//                }
//            }
//        }

//        [Column(Storage = "_TotalPurchase", DbType = "Decimal(38,2)")]
//        public System.Nullable<decimal> TotalPurchase
//        {
//            get
//            {
//                return this._TotalPurchase;
//            }
//            set
//            {
//                if ((this._TotalPurchase != value))
//                {
//                    this._TotalPurchase = value;
//                }
//            }
//        }
//    }

//    public partial class TenMostExpensiveProductsResult
//    {

//        private string _TenMostExpensiveProducts;

//        private System.Nullable<decimal> _UnitPrice;

//        public TenMostExpensiveProductsResult()
//        {
//        }

//        [Column(Storage = "_TenMostExpensiveProducts", DbType = "NVarChar(40)")]
//        public string TenMostExpensiveProducts
//        {
//            get
//            {
//                return this._TenMostExpensiveProducts;
//            }
//            set
//            {
//                if ((this._TenMostExpensiveProducts != value))
//                {
//                    this._TenMostExpensiveProducts = value;
//                }
//            }
//        }

//        [Column(Storage = "_UnitPrice", DbType = "Money")]
//        public System.Nullable<decimal> UnitPrice
//        {
//            get
//            {
//                return this._UnitPrice;
//            }
//            set
//            {
//                if ((this._UnitPrice != value))
//                {
//                    this._UnitPrice = value;
//                }
//            }
//        }
//    }

//    public partial class WholeCustomersSetResult
//    {

//        private string _CustomerID;

//        private string _CompanyName;

//        private string _ContactName;

//        private string _ContactTitle;

//        private string _Address;

//        private string _City;

//        private string _Region;

//        private string _PostalCode;

//        private string _Country;

//        private string _Phone;

//        private string _Fax;

//        public WholeCustomersSetResult()
//        {
//        }

//        [Column(Storage = "_CustomerID", DbType = "NChar(5)")]
//        public string CustomerID
//        {
//            get
//            {
//                return this._CustomerID;
//            }
//            set
//            {
//                if ((this._CustomerID != value))
//                {
//                    this._CustomerID = value;
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40)")]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this._CompanyName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ContactName", DbType = "NVarChar(30)")]
//        public string ContactName
//        {
//            get
//            {
//                return this._ContactName;
//            }
//            set
//            {
//                if ((this._ContactName != value))
//                {
//                    this._ContactName = value;
//                }
//            }
//        }

//        [Column(Storage = "_ContactTitle", DbType = "NVarChar(30)")]
//        public string ContactTitle
//        {
//            get
//            {
//                return this._ContactTitle;
//            }
//            set
//            {
//                if ((this._ContactTitle != value))
//                {
//                    this._ContactTitle = value;
//                }
//            }
//        }

//        [Column(Storage = "_Address", DbType = "NVarChar(60)")]
//        public string Address
//        {
//            get
//            {
//                return this._Address;
//            }
//            set
//            {
//                if ((this._Address != value))
//                {
//                    this._Address = value;
//                }
//            }
//        }

//        [Column(Storage = "_City", DbType = "NVarChar(15)")]
//        public string City
//        {
//            get
//            {
//                return this._City;
//            }
//            set
//            {
//                if ((this._City != value))
//                {
//                    this._City = value;
//                }
//            }
//        }

//        [Column(Storage = "_Region", DbType = "NVarChar(15)")]
//        public string Region
//        {
//            get
//            {
//                return this._Region;
//            }
//            set
//            {
//                if ((this._Region != value))
//                {
//                    this._Region = value;
//                }
//            }
//        }

//        [Column(Storage = "_PostalCode", DbType = "NVarChar(10)")]
//        public string PostalCode
//        {
//            get
//            {
//                return this._PostalCode;
//            }
//            set
//            {
//                if ((this._PostalCode != value))
//                {
//                    this._PostalCode = value;
//                }
//            }
//        }

//        [Column(Storage = "_Country", DbType = "NVarChar(15)")]
//        public string Country
//        {
//            get
//            {
//                return this._Country;
//            }
//            set
//            {
//                if ((this._Country != value))
//                {
//                    this._Country = value;
//                }
//            }
//        }

//        [Column(Storage = "_Phone", DbType = "NVarChar(24)")]
//        public string Phone
//        {
//            get
//            {
//                return this._Phone;
//            }
//            set
//            {
//                if ((this._Phone != value))
//                {
//                    this._Phone = value;
//                }
//            }
//        }

//        [Column(Storage = "_Fax", DbType = "NVarChar(24)")]
//        public string Fax
//        {
//            get
//            {
//                return this._Fax;
//            }
//            set
//            {
//                if ((this._Fax != value))
//                {
//                    this._Fax = value;
//                }
//            }
//        }
//    }

//    public partial class PartialCustomersSetResult
//    {

//        private string _CustomerID;

//        private string _ContactName;

//        private string _CompanyName;

//        public PartialCustomersSetResult()
//        {
//        }

//        [Column(Storage = "_CustomerID", DbType = "NChar(5)")]
//        public string CustomerID
//        {
//            get
//            {
//                return this._CustomerID;
//            }
//            set
//            {
//                if ((this._CustomerID != value))
//                {
//                    this._CustomerID = value;
//                }
//            }
//        }

//        [Column(Storage = "_ContactName", DbType = "NVarChar(30)")]
//        public string ContactName
//        {
//            get
//            {
//                return this._ContactName;
//            }
//            set
//            {
//                if ((this._ContactName != value))
//                {
//                    this._ContactName = value;
//                }
//            }
//        }

//        [Column(Storage = "_CompanyName", DbType = "NVarChar(40)")]
//        public string CompanyName
//        {
//            get
//            {
//                return this._CompanyName;
//            }
//            set
//            {
//                if ((this._CompanyName != value))
//                {
//                    this._CompanyName = value;
//                }
//            }
//        }
//    }
//}

