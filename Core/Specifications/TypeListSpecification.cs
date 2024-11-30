using Core.Enities;
namespace Core.Specifications
{
    public class TypeListSpecification : BaseSpecification<Product, string>
    {
        public TypeListSpecification() 
        {
            AddSelect(x => x.Type);
            ApplyDistinct();
        }
    }
}
