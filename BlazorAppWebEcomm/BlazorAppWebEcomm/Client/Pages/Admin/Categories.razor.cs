using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Pages.Admin
{
    public partial class Categories : ComponentBase,IDisposable
    {

        [Inject]
        public ICategoryService categoryService { get; set; }

        private Category editingCategory { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await categoryService.GetAdminCategories();
            categoryService.OnChange += StateHasChanged;
        }
        public void Dispose()
        {
            categoryService.OnChange -= StateHasChanged;
        }
        private void CreateNewCategory()
        {
            editingCategory = categoryService.CreateNewCategory();
        }
        private void EditCategory(Category category)
        {
            category.Editing = true;
            editingCategory=category;
        }
        private async Task UpdateCategory()
        {
            if(editingCategory.IsNew==true)
            {
                await categoryService.AddCategory(editingCategory);
            }
            else
            {
                await categoryService.UpdateCategory(editingCategory);
            }
            editingCategory = new Category();
        }
        private async Task CancelEditing()
        {
            editingCategory = new Category();
            await categoryService.GetAdminCategories();
        }
        private async Task DeleteCategory(int Id)
        {
            await categoryService.DeleteCategory(Id);
        }

    }
}
