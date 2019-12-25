using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.CodeFiles
{
    public partial class ToDoOperation : ComponentBase
    {
        public bool ShowModel = false;
        public bool ShowAlert = false;
        public bool ShowModeletePopup = false;
        public string OperationStatusText = "";
        public string PopupTitle = "";

        public BlazorClient.Data.ToDoDataItem ToDoDataItem = null;
        public string ActionText = "";

        public ToDoGrpcService.ToDoItems toDoItems;

        public string DeleteItemId { get; set; }

        [Inject]
        protected BlazorClient.Services.ToDoDataService ToDoService { get; set; } 


        protected override void OnInitialized()
        {

            GetToDoList();
        }

        protected void GetToDoList()
        {

            toDoItems= ToDoService.GetToDoList();
        }

        protected async Task ShowEditForm(int Id)
        {
            
            PopupTitle = "To Do Edit";
            ActionText = "Update";

            ToDoDataItem = ToDoService.GetToDoItem(Id);
            ShowModel = true;
        }

        protected void ShowAddpopup()
        {
            ToDoDataItem = new Data.ToDoDataItem() { Title = "", Description = "", Status = false, Id = 0 };
            PopupTitle = "To Do Add";
            ActionText = "Add";
            ShowModel = true;
        }
        protected void ShowDeletePopup(string Id)
        {
            DeleteItemId = Id;
            ShowModeletePopup = true;
        }



        protected void PostData()
        {
            bool status = false;
            if (ToDoDataItem.Id > 0)
            {
                status = ToDoService.UpdateToDoData(this.ToDoDataItem);

            }
            else
            {
                status = ToDoService.AddToDoData(this.ToDoDataItem);
            }
            Reload(status);
        }

        public void DeleteData()
        {
            
            var operationStatus = ToDoService.DeleteData(DeleteItemId);
            Reload(operationStatus);
        }

        protected void Reload(bool status)
        {
            ShowModeletePopup = false;
            ShowModel = false;
            GetToDoList();
            ShowAlert = true;
            if (status)
            {
                OperationStatusText = "Processed Successfully !! ";
            }
            else
            {
                OperationStatusText = "Error Occured  ";
            }

        }

        protected void DismissPopup()
        {
            ShowModel = false;
            ShowAlert = false;
            ShowModeletePopup = false;
        }

    }
}
