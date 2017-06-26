using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp;

namespace PORTFOLIO.api.Controllers.api
{
    [Route("api/stats")]
    [ResponseCache(Duration = 7200)]
    public class Stats : Controller
    {
        private readonly IConfigurationRoot config;

        public Stats(IConfigurationRoot config)
        {
            this.config = config;
        }

        public async Task<IActionResult> GetStats()
        {
            var stats = new StatsModel();
            stats.codingStats = await GetCodingStats();
            stats.snowboardStats = await GetSnowboardStats();

            return Ok(stats);
        }

        private async Task<SnowboardStats> GetSnowboardStats()
        {
            var username = config.GetSection("epicMixUsername").Value;
            var password = config.GetSection("epicMixPassword").Value;

            var baseAddress = new Uri("https://www.epicmix.com/vailresorts/sites/epicmix/");
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                });

                var loginResult = client.PostAsync("handlers/authentication.ashx", content).Result;
                var headers = loginResult.Headers.ToList();

                loginResult.EnsureSuccessStatusCode();

                var dataResult = await client.GetAsync("api/Stats/VerticalFeetTracker.ashx").Result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<SnowboardStats>(dataResult);
            }
        }

        private async Task<List<CodingStat>> GetCodingStats()
        {
            var stackoverflowStats = await GetStackoverflowStats();
            var githubStats = await GetGithubStats();

            return stackoverflowStats.Concat(githubStats).ToList();
        }

        private async Task<List<CodingStat>> GetGithubStats()
        {
            using (var client = new HttpClient())
            {
                var config = Configuration.Default.WithDefaultLoader();
                var address = "https://github.com/KolbySisk";
                var document = await BrowsingContext.New(config).OpenAsync(address);
                var repositorySelector = ".underline-nav-item:nth-of-type(2) .Counter";
                var repositoryValue = document.QuerySelectorAll(repositorySelector)[0].TextContent;

                var statRepository = new CodingStat();
                statRepository.href = "https://github.com/KolbySisk";
                statRepository.src = "images/icon-github.png";
                statRepository.alt = "github logo";
                statRepository.displayValue = 0;
                statRepository.value = Regex.Replace(repositoryValue, "[^0-9]", "");
                statRepository.title = "public repositories";

                var codingStatList = new List<CodingStat>();
                codingStatList.Add(statRepository);

                return codingStatList;
            }
        }

        private async Task<List<CodingStat>> GetStackoverflowStats()
        {
            using (var client = new HttpClient())
            {
                var config = Configuration.Default.WithDefaultLoader();
                var address = "http://stackoverflow.com/users/1933563/kolby";
                var document = await BrowsingContext.New(config).OpenAsync(address);

                var repSelector = ".reputation";
                var repValue = document.QuerySelectorAll(repSelector)[0].TextContent;
                var statRep = new CodingStat();
                statRep.href = "http://stackoverflow.com/users/1933563/kolby";
                statRep.src = "images/icon-stackoverflow.png";
                statRep.alt = "stackoverflow logo";
                statRep.displayValue = 0;
                statRep.value = Regex.Replace(repValue, "[^0-9]", "");
                statRep.title = "reputation";

                var answersSelector = ".answers .number";
                var answersValue = document.QuerySelectorAll(answersSelector)[0].TextContent;
                var statAnswers = new CodingStat();
                statAnswers.href = "http://stackoverflow.com/users/1933563/kolby";
                statAnswers.src = "images/icon-stackoverflow.png";
                statAnswers.alt = "stackoverflow logo";
                statAnswers.displayValue = 0;
                statAnswers.value = answersValue;
                statAnswers.title = "answers";

                var peopleReachedSelector = ".people-helped .number";
                var peopleReachedValue = document.QuerySelectorAll(peopleReachedSelector)[0].TextContent;
                var statPeopleReached = new CodingStat();
                statPeopleReached.href = "http://stackoverflow.com/users/1933563/kolby";
                statPeopleReached.src = "images/icon-stackoverflow.png";
                statPeopleReached.alt = "stackoverflow logo";
                statPeopleReached.displayValue = 0;
                statPeopleReached.value = peopleReachedValue.Replace("~", "").Replace("k", "000");
                statPeopleReached.title = "people reached";

                var codingStatList = new List<CodingStat>();
                codingStatList.Add(statRep);
                codingStatList.Add(statAnswers);
                codingStatList.Add(statPeopleReached);

                return codingStatList;
            }
        }
    }

    public class StatsModel
    {
        public SnowboardStats snowboardStats { get; set; }
        public List<CodingStat> codingStats { get; set; }
    }

    public class CodingStat
    {
        public string href { get; set; }
        public string src { get; set; }
        public string alt { get; set; }
        public int displayValue { get; set; }
        public string value { get; set; }
        public string title { get; set; }
    }

    public class SnowboardStats
    {
        public int VerticalFeetMin { get; set; }
        public int VerticalFeetMax { get; set; }
        public PagedResult PagedResult { get; set; }
        public ProfileStats ProfileStats { get; set; }
    }

    public class List
    {
        public string Date { get; set; }
        public string ResortName { get; set; }
        public int ResortId { get; set; }
        public int ResortDayStatId { get; set; }
        public int TimeTagId { get; set; }
        public int PhotosProCaptured { get; set; }
        public int PhotosUgcCaptured { get; set; }
        public int VerticalFeet { get; set; }
        public int VerticalFeetEdited { get; set; }
        public int TotalVerticalFeet { get; set; }
        public int TotalPhotos { get; set; }
        public int PinCount { get; set; }
        public int LiftCount { get; set; }
        public int Points { get; set; }
        public int Medals { get; set; }
        public int Lessons { get; set; }
        public int AcademyLevel { get; set; }
        public string Discipline { get; set; }
    }

    public class PagedResult
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public bool HasCredits { get; set; }
        public List<List> List { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string OrderBy { get; set; }
        public int TotalCount { get; set; }
        public int AbsoluteCount { get; set; }
        public int TotalPages { get; set; }
        public int Relationship { get; set; }
    }

    public class ProfileStats
    {
        public int VerticalFeet { get; set; }
        public int Points { get; set; }
        public int CheckIns { get; set; }
        public string Discipline { get; set; }
        public int AcademyLevel { get; set; }
        public int Photos { get; set; }
        public int Pins { get; set; }
        public int Lifts { get; set; }
        public int DaysOnMountain { get; set; }
        public string MostVisitedResortName { get; set; }
        public int MostVisitedResortId { get; set; }
        public string SummaryName { get; set; }
        public int TimeTagId { get; set; }
        public int MountainsVisited { get; set; }
        public string Medals { get; set; }
        public string LVPoints { get; set; }
    }
}
