﻿using Atlass.Framework.Common.Log;
using Atlass.Framework.ViewModels.YmlConfigs;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Atlass.Framework.Core
{
    public class GlobalContext
    {
        public static IWebHostEnvironment HostingEnvironment { get; set; }

        public static string GetVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return version.Major + "." + version.Minor;
        }

        /// <summary>
        /// 网站根目录
        /// </summary>
        public static string ContentRootPath { get { return HostingEnvironment.ContentRootPath; } }

        /// <summary>
        /// 网站资源目录 wwwroot
        /// </summary>
        public static string  WebRootPath{ get { return HostingEnvironment.WebRootPath; } }

        /// <summary>
        /// 运行环境 0-开发，1-发布
        /// </summary>
        public static int RuntimeEnvironment { get; set; } = 1;
        /// <summary>
        /// freesql的配置
        /// </summary>
        public static FreeSqlConfig FreeSqlConfig { get; set; } = new FreeSqlConfig();
        /// <summary>
        /// 数据库配置
        /// </summary>
        public static List<DbConfigsDto> DbConfigs { get; set; }

        /// <summary>
        /// 默认数据库配置
        /// </summary>
        public static DbConfigsDto DefaultDbConfig { get; set; }
        /// <summary>
        /// redis配置
        /// </summary>
        public static RedisConfigDto RedisConfig { get; set; }

        /// <summary>
        /// 定时任务配置
        /// </summary>
        public static CrontabConfigDto CrontabConfigDto { get; set; }

    }
}
