namespace BlazorAppWebEcomm.Server.Services.ProductTypeServices
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly EcommDatabaseContext context;

        public ProductTypeService(EcommDatabaseContext context)
        {
            this.context = context;
        }

        public async Task<ServiceResponse<List<Models.ProductType>>> AddProductTypes(Models.ProductType productType)
        {

           context.ProductTypes.Add(productType);
            await context.SaveChangesAsync();
            return await  GetProductTypes();
        }

        public async Task<ServiceResponse<List<Models.ProductType>>> GetProductTypes()
        {
            var productTypes = await context.ProductTypes.ToListAsync();
            return new ServiceResponse<List<Models.ProductType>> { Data = productTypes };
        }

        public async Task<ServiceResponse<List<Models.ProductType>>> UpdateProductTypes(Models.ProductType productType)
        {
            var dbProductType =await  context.ProductTypes.FindAsync(productType.Id);
            if(dbProductType==null)
            {
                return new ServiceResponse<List<Models.ProductType>>
                {
                    Success = false,
                    Message = "Product Type not exist."
                };
            }
            dbProductType.Name = productType.Name;
            await context.SaveChangesAsync();
            return  await GetProductTypes();
        }
    }
}
