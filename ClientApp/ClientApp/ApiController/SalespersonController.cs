﻿using ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ApiController
{
    class SalespersonController : IRestController<Salesperson>
    {
        public string Endpoint { get; set; }
        private RestClient<Salesperson> client;

        public SalespersonController()
        {
            client = new RestClient<Salesperson>();
        }

        public async Task<Salesperson> GetAsync(int id)
        {
            string path = $"/api/salesperson/{id}";
            client.Endpoint = Endpoint;
            Salesperson person = await client.MakeRequest(path);
            return person;
        }

        public Task<IEnumerable<Salesperson>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task PersistAsync(Salesperson t)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Salesperson t)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Salesperson t)
        {
            throw new NotImplementedException();
        }
    }
}
