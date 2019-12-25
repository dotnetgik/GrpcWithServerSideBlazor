using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorClient.Services
{
    public class ToDoDataService
    {


        private ToDoGrpcService.ToDoService.ToDoServiceClient GetServiceClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            return new ToDoGrpcService.ToDoService.ToDoServiceClient(channel);
        }

        public bool AddToDoData(Data.ToDoDataItem toDoDataItem)
        {

            var client = GetServiceClient();

            var todoData = new ToDoGrpcService.ToDoData()
            {
                Status = toDoDataItem.Status,
                Title = toDoDataItem.Title,
                Description = toDoDataItem.Description
            };

            var response = client.PostToDoItem(todoData, null);
            return response.Status;

        }
        public bool UpdateToDoData(Data.ToDoDataItem toDoDataItem)
        {
            var client = GetServiceClient();
            var updateData = new ToDoGrpcService.ToDoPutQuery();
            updateData.Id = toDoDataItem.Id;
            updateData.ToDoDataItem = new ToDoGrpcService.ToDoData()
            {
                Id = toDoDataItem.Id,
                Status = toDoDataItem.Status,
                Title = toDoDataItem.Title,
                Description = toDoDataItem.Description
            };
            var response = client.PutToDoItem(updateData, null);
            return response.Status;
        }
        public bool DeleteData(string ToDoId)
        {
            var client = GetServiceClient();
            var response = client.DeleteItem(new ToDoGrpcService.ToDoQuery() { Id = Convert.ToInt32(ToDoId) }, null);
            return response.Status;
        }
        public ToDoGrpcService.ToDoItems GetToDoList()
        {
            var client = GetServiceClient();
            return  client.GetToDo(new Google.Protobuf.WellKnownTypes.Empty(), null);
            
        }

        public Data.ToDoDataItem GetToDoItem(int id)
        {
            var client = GetServiceClient();
            var todoItem = client.GetToDoItem(new ToDoGrpcService.ToDoQuery() { Id = Convert.ToInt32(id) }, null);

            return new Data.ToDoDataItem() { Title = todoItem.Title, Description = todoItem.Description, Status = todoItem.Status, Id = todoItem.Id };

        }
    }
}
