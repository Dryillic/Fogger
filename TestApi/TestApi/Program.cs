﻿using Fogger;
using Fogger.Models;
using System;
using System.Collections.Generic;

namespace TestApi
{
    //Test project integrating API
    class Program
    {
        static void Main(string[] args)
        {
            //Semi colon seperated fogbugz enviroment variable in the form of domain;user
            var SuperSecretLogin = Environment.GetEnvironmentVariable("Fogbugz").Split(';');
            var Domain = SuperSecretLogin[0];
            var UserName = SuperSecretLogin[1];
            Console.Write("Enter Password:"); var Password = Console.ReadLine();

            //Initialize api and login
            FoggerApiWrapper OurFogBugzAPI = new FoggerApiWrapper(Domain, UserName, Password);

            //Get the current filters availiable
            List<Filter> filters = OurFogBugzAPI.GetFilters();

            //Sets the current filter in the api and the wrapper
            OurFogBugzAPI.SetCurrentFilter(filters.Find(x => (x.sFilter == "377")));

            //Internally this calls Search("") with an empty query
            List<Case> cases = OurFogBugzAPI.SearchCurrentFilter();

            //Single case from list
            Case caseToEdit = cases[0];

            //Change Area
            caseToEdit.Area.Value = "MyTestArea";

            //Edit Case
            OurFogBugzAPI.CaseManager.EditCase(caseToEdit);

            //Always logoff to invalidate token. It is recommended to reuse the api token as much
            //as possible after its generated by a login. For now we will just logoff.
            OurFogBugzAPI.Logoff();
        }

    }
}
