﻿using Azure.Search.Documents.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAzureSearch.Pages
{
    public class SearchModel : PageModel
    {
        private readonly SearchProvider _searchProvider;
        private readonly ILogger<IndexModel> _logger;

        public string SearchText { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int LeftMostPage { get; set; }
        public int PageRange { get; set; }
        public string Paging { get; set; }
        public int PageNo { get; set; }
        public SearchResults<PersonCity> PersonCities;

        public SearchModel(SearchProvider searchProvider,
            ILogger<IndexModel> logger)
        {
            _searchProvider = searchProvider;
            _logger = logger;
        }

        public void OnGet()
        {

        }


        public async Task<ActionResult> OnPostPageAsync(SearchData model)
        {
            int page;

            switch (model.Paging)
            {
                case "prev":
                    page = PageNo - 1;
                    break;

                case "next":
                    page = PageNo + 1;
                    break;

                default:
                    page = int.Parse(model.Paging);
                    break;
            }

            int leftMostPage = LeftMostPage;

            model.SearchText = SearchText;

            await _searchProvider.RunQueryAsync(model, page, leftMostPage).ConfigureAwait(false);

            PageNo = page;
            SearchText = model.SearchText;
            LeftMostPage = model.LeftMostPage;
  

            return Page();
        }

    }
}
