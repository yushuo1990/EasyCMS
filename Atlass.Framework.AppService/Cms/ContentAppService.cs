﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//     Website: http://www.freesql.net
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlass.Framework.Cache;
using Atlass.Framework.Common;
using Atlass.Framework.Models;
using Atlass.Framework.ViewModels.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Atlass.Framework.Models
{


    public partial class ContentAppService
    {
        private readonly IFreeSql Sqldb;
        public ContentAppService(IServiceProvider service)
        {
            Sqldb = service.GetRequiredService<IFreeSql>();
        }


        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="channelId">栏目</param>
        /// <param name="title">文章标题</param>
        /// <param name="status">审核状态</param>
        /// <param name="contentProperty">0-全部，1-置顶，2-推荐</param>
        /// <returns></returns>
        public DataTableDto GetData(DataTableDto param, int channelId, string title, int status, int contentProperty)
        {
            long total = 0;
            var query = Sqldb.Select<cms_content>()
                .WhereIf(channelId > 0, s => s.channel_id == channelId)
                .WhereIf(status >= 0, s => s.content_status == status)
                .WhereIf(!title.IsEmpty(),s=>s.title.Contains(title));
            if (contentProperty == 1)
            {
                query = query.Where(s => s.is_top == 1);
            }
            else if (contentProperty == 2)
            {
                query = query.Where(s => s.is_recommend == 1);
            }
            var data = query.OrderByDescending(s => s.is_top)
             .OrderByDescending(s => s.id)
             .Page(param.pageNumber, param.pageSize)
             .Count(out total)
             .ToList();

            param.total = total;
            param.rows = data;
            return param;
        }


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="dto"></param>
        public void SaveContent(cms_content dto, LoginUserDto loginUser)
        {
            dto.sub_title = dto.sub_title ?? "";
            dto.summary = dto.summary ?? "";
            dto.content = dto.content ?? "";
            dto.author = dto.author ?? "";
            dto.source = dto.source ?? "";
            dto.content_href = dto.content_href ?? "";
            dto.cover_image = dto.cover_image ?? "";
            dto.update_by = loginUser.LoginName;
            dto.update_time = DateTime.Now;
            if (dto.id == 0)
            {
                dto.dept_id = loginUser.DeptId;
                dto.create_by = loginUser.LoginName;
                dto.create_time = dto.update_time;

                long contentId = Sqldb.Insert(dto).ExecuteIdentity();
                ChannelManagerCache.SetChannelLink(dto.channel_id, (int)contentId);
                //生成文章
                //GenerateContent generate = new GenerateContent();
                //generate.CreateHtml((int)contentId);
            }
            else
            {

                Sqldb.Update<cms_content>().SetSource(dto).ExecuteAffrows();
                //生成文章
                // GenerateContent generate = new GenerateContent();
                //generate.CreateHtml(dto.id);
            }
        }


        /// <summary>
        ///获取单条数据
        /// </summary>
        /// <param name="id"></param>

        public cms_content GetDataById(int id)
        {
            return Sqldb.Select<cms_content>().Where(s => s.id == id).First();
        }

        /// <summary>
        ///批量删除
        /// </summary>
        /// <param name="ids"></param>

        public void DelByIds(string ids)
        {
            var idsArray = ids.SplitToArrayInt();
            Sqldb.Delete<cms_content>().Where(s => idsArray.Contains(s.id)).ExecuteAffrows();
        }

        /// <summary>
        /// 推荐
        /// </summary>
        /// <param name="id"></param>
        public void SetTop(string ids)
        {
            var idsArray = ids.SplitToArrayInt();
            Sqldb.Update<cms_content>().Set(s => s.is_top, 1).Where(s => idsArray.Contains(s.id)).ExecuteAffrows();
        }
        /// <summary>
        /// 推荐
        /// </summary>
        /// <param name="id"></param>
        public void SetRecomend(string ids)
        {
            var idsArray = ids.SplitToArrayInt();
            Sqldb.Update<cms_content>().Set(s => s.is_recommend, 1).Where(s => idsArray.Contains(s.id)).ExecuteAffrows();
        }

    }

}



