using AutoMapper;
using QuickSearchData;
using QuickSearchWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickSearchWeb.App_Start
{
    public static class MappingConfig
    {
        public static void RegisterMaps()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<ProductViewModel, Product>();
                cfg.CreateMap<Product, ProductViewModel>();

                cfg.CreateMap<UserViewModel, ApplicationUser>();
                cfg.CreateMap<ApplicationUser, UserViewModel>();

                cfg.CreateMap<TimeSheetSummary, TimeSheetSummaryViewModel>();
                cfg.CreateMap<TimeSheetSummaryViewModel, TimeSheetSummary>().ForMember(dto => dto.Day1Hours, opt => opt.NullSubstitute(0)).ForMember(dto => dto.Day2Hours, opt => opt.NullSubstitute(0)).ForMember(dto => dto.Day3Hours, opt => opt.NullSubstitute(0)).ForMember(dto => dto.Day4Hours, opt => opt.NullSubstitute(0)).ForMember(dto => dto.Day5Hours, opt => opt.NullSubstitute(0)).ForMember(dto => dto.Day6Hours, opt => opt.NullSubstitute(0)).ForMember(dto => dto.Day7Hours, opt => opt.NullSubstitute(0));

                cfg.CreateMap<TimeSheetListViewModel, TimeSheetsMaster>();
                cfg.CreateMap<TimeSheetsMaster, TimeSheetListViewModel>()/*.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.AspNetUser.UserName))*/;

                cfg.CreateMap<TimeSheetViewModel, TimeSheetsMaster>();
                cfg.CreateMap<TimeSheetsMaster, TimeSheetViewModel>();

                cfg.CreateMap<LoaViewModel, LOA>();
                cfg.CreateMap<LOA, LoaViewModel>();

                cfg.CreateMap<LoaListViewModel, LOA>();
                cfg.CreateMap<LOA, LoaListViewModel>();

                cfg.CreateMap<ExpenseViewModel, ExpenseMaster>();
                cfg.CreateMap<ExpenseMaster, ExpenseViewModel>();

                cfg.CreateMap<ExpenseSummaryViewModel, ExpenseSummary>();
                cfg.CreateMap<ExpenseSummary, ExpenseSummaryViewModel>();

                cfg.CreateMap<ExpenseListViewModel, ExpenseMaster>();
                cfg.CreateMap<ExpenseMaster, ExpenseListViewModel>();

                cfg.CreateMap<SupportViewModel, SupportMaster>();
                cfg.CreateMap<SupportMaster, SupportViewModel>();

                cfg.CreateMap<SupportListViewModel, SupportMaster>();
                cfg.CreateMap<SupportMaster, SupportListViewModel>();
            });

        }
    }
}