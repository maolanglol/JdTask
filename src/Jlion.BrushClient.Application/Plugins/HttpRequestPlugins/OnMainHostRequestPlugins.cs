using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jlion.BrushClient.Application.Model;
using Jlion.BrushClient.Framework;
using Jlion.BrushClient.Framework.Helper;

namespace Jlion.BrushClient.Application.Plugins
{
    public class OnMainHostRequestPlugins : OnHttpRequestPlugins
    {
        OnAppSettingPlugins _appSettings;

        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public OnMainHostRequestPlugins(OnAppSettingPlugins appSetting)
            : base(appSetting.PluginsHost)
        {
            _appSettings = appSetting;
        }

        private void DoHandler(string accessToken)
        {
            dict = new Dictionary<string, string>();
            //var accessToken = ServiceIniCacheHelper.ReadAccessToken();
            //var refreshToken = ServiceIniCacheHelper.ReadRefreshToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                dict.Add("Authorization", $"Bearer {accessToken}");
            }
            dict.Add("version", _appSettings.VersionCode.ToString());
        }

        #region 用户登陆注册
        public async Task<DataResponse<AuthResponse>> AuthAsync(AuthRequest request)
        {
            try
            {
                DoHandler("");
                var hkv = ToKeyValuePair().ToArray();
                return await PostAsync<DataResponse<AuthResponse>>(ConstDefintion.Auth_Url, JsonConvert.SerializeObject(request),hkvs:hkv);
            }
            catch (Exception ex)
            {
                TextHelper.Error($"AuthAsync 异常", ex);
                return DataResponse<AuthResponse>.AsFail(message: ex.Message);
            }
        }

        public async Task<DataResponse<bool>> RegisterAsync(UserRegRequest request)
        {
            try
            {
                DoHandler("");
                var hkv = ToKeyValuePair().ToArray();
                return await PostAsync<DataResponse<bool>>(ConstDefintion.User_Reg_Url, JsonConvert.SerializeObject(request),hkvs: hkv);
            }
            catch (Exception ex)
            {
                TextHelper.Error($"RegisterAsync 异常", ex);
                return DataResponse<bool>.AsFail(message: ex.Message);
            }
        }
        #endregion

        #region 获得用户账户信息
        public async Task<DataResponse<AccountItemResponse>> QueryAccountAsync(string accessToken)
        {
            try
            {
                DoHandler(accessToken);
                var hkv = ToKeyValuePair().ToArray();
                return await Get<DataResponse<AccountItemResponse>>(ConstDefintion.Account_Url, hkvs: hkv);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("401"))
                    return DataResponse<AccountItemResponse>.AsFail(Enums.ApiCodeEnums.ERROR_NOLOGIN);
                TextHelper.Error($"QueryAccountAsync 异常", ex);
                return DataResponse<AccountItemResponse>.AsFail(message: ex.Message);
            }
        }
        #endregion

        #region 获取产品信息
        public async Task<DataResponse<List<ProductItemResponse>>> GetProductListAsync(string accessToken)
        {
            try
            {
                DoHandler(accessToken);
                var hkv = ToKeyValuePair().ToArray();
                return await Get<DataResponse<List<ProductItemResponse>>>(ConstDefintion.Product_Query_Url, hkvs: hkv);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("401"))
                    return DataResponse<List<ProductItemResponse>>.AsFail(Enums.ApiCodeEnums.ERROR_NOLOGIN);

                TextHelper.Error($"GetProductListAsync 异常", ex);
                return DataResponse<List<ProductItemResponse>>.AsFail(message: ex.Message);
            }
        }
        #endregion

        #region 提现申请
        /// <summary>
        /// 提现申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> WithdrawAsync(WithdrawRequest request)
        {
            try
            {
                DoHandler(request.AccessToken);
                var hkv = this.ToKeyValuePair().ToArray();
                return await PostAsync<DataResponse<bool>>(ConstDefintion.Withdraw_Url, JsonConvert.SerializeObject(request), hkvs: hkv);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("401"))
                    return DataResponse<bool>.AsFail(Enums.ApiCodeEnums.ERROR_NOLOGIN);

                TextHelper.Error($"WithdrawAsync 异常", ex);
                return DataResponse<bool>.AsFail(message: ex.Message);
            }
        }
        #endregion

        #region 提现记录
        /// <summary>
        /// 提现申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataResponse<PageData<WithdrawRecordResponse>>> WithdrawRecordListAsync(WithdrwaRecordGetRequest request)
        {
            try
            {
                DoHandler(request.AccessToken);
                var hkv = this.ToKeyValuePair().ToArray();
                var page = nameof(request.Page).AsPair(request.Page.ToString());
                var rows = nameof(request.Rows).AsPair(request.Rows.ToString());
                var type = nameof(request.Types).AsPair(((int)request.Types).ToString());
                var status = nameof(request.Status).AsPair(((int)request.Status).ToString());
                var gteTime = nameof(request.GteTime).AsPair(request.GteTime.ToString());
                var lteTime = nameof(request.LteTime).AsPair(request.LteTime.ToString());
                var param = new List<KeyValuePair<string, string>>();
                param.Add(page);
                param.Add(rows);
                param.Add(type);
                param.Add(status);
                param.Add(gteTime);
                param.Add(lteTime);


                return await Get<DataResponse<PageData<WithdrawRecordResponse>>>(ConstDefintion.Withdraw_Url, hkvs: hkv, param.ToArray());
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("401"))
                    return DataResponse<PageData<WithdrawRecordResponse>>.AsFail(Enums.ApiCodeEnums.ERROR_NOLOGIN);

                TextHelper.Error($"WithdrawRecordListAsync 异常", ex);
                return DataResponse<PageData<WithdrawRecordResponse>>.AsFail(message: ex.Message);
            }
        }
        #endregion

        #region 获得系统配置
        public async Task<DataResponse<SystemSettingsResponse>> QuerySettingsAsync(string accessToken)
        {
            try
            {
                DoHandler(accessToken);
                var hkv = ToKeyValuePair().ToArray();
                return await Get<DataResponse<SystemSettingsResponse>>(ConstDefintion.Settings_Url, hkvs: hkv);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("401"))
                    return DataResponse<SystemSettingsResponse>.AsFail(Enums.ApiCodeEnums.ERROR_NOLOGIN);
                TextHelper.Error($"QueryAccountAsync 异常", ex);
                return DataResponse<SystemSettingsResponse>.AsFail(message: ex.Message);
            }
        }
        #endregion

        #region 获得资金流水
        public async Task<DataResponse<PageData<AccountRecordResponse>>> QueryAccountRecorrdAsync(AccountRecordRequest request)
        {
            try
            {
                DoHandler(request.AccessToken);
                var hkv = this.ToKeyValuePair().ToArray();
                var page = nameof(request.Page).AsPair(request.Page.ToString());
                var rows = nameof(request.Rows).AsPair(request.Rows.ToString());
                var type = nameof(request.Type).AsPair((request.Type).ToString());
                var gteTime = nameof(request.GteTime).AsPair(request.GteTime.ToString());
                var lteTime = nameof(request.LteTime).AsPair(request.LteTime.ToString());
                var param = new List<KeyValuePair<string, string>>();
                param.Add(page);
                param.Add(rows);
                param.Add(type);
                param.Add(gteTime);
                param.Add(lteTime);

                return await Get<DataResponse<PageData<AccountRecordResponse>>>(ConstDefintion.AcccountRecord_Url, hkvs: hkv, param.ToArray());
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("401"))
                    return DataResponse<PageData<AccountRecordResponse>>.AsFail(Enums.ApiCodeEnums.ERROR_NOLOGIN);

                TextHelper.Error($"WithdrawRecordListAsync 异常", ex);
                return DataResponse<PageData<AccountRecordResponse>>.AsFail(message: ex.Message);
            }
        }
        #endregion

        #region 获取任务
        /// <summary>
        /// 获取任务
        /// </summary>
        /// <returns></returns>
        public async Task<DataResponse<List<TaskItemResponse>>> GetTaskAsync(string accessToken)
        {
            try
            {
                DoHandler(accessToken);
                var hkv = this.ToKeyValuePair().ToArray();

                return await Get<DataResponse<List<TaskItemResponse>>>(ConstDefintion.Task_Url, hkvs: hkv);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("401"))
                    return DataResponse<List<TaskItemResponse>>.AsFail(Enums.ApiCodeEnums.ERROR_NOLOGIN);

                TextHelper.Error($"GetTaskAsync 异常", ex);
                return DataResponse<List<TaskItemResponse>>.AsFail(message: ex.Message);
            }
        }

        /// <summary>
        /// 提交任务
        /// </summary>
        /// <returns></returns>
        public async Task<DataResponse<TaskComplateResponse>> PostTaskAsync(long id, string accessToken)
        {
            try
            {
                DoHandler(accessToken);
                var hkv = this.ToKeyValuePair().ToArray();
                return await PostAsync<DataResponse<TaskComplateResponse>>($"{ConstDefintion.Task_Url}/{id}", hkvs: hkv);
            }
            catch (Exception ex)
            {
                TextHelper.Error($"PostTaskAsync 异常", ex);
                return DataResponse<TaskComplateResponse>.AsFail(message: ex.Message);
            }
        }
        #endregion

        #region 预下单
        public async Task<DataResponse<string>> GetPreOrderAsync(long productId, string accessToken)
        {
            try
            {
                DoHandler(accessToken);
                var hkv = this.ToKeyValuePair().ToArray();
                return await Get<DataResponse<string>>($"{ConstDefintion.Pay_Url}/{productId}", hkvs: hkv);
            }
            catch (Exception ex)
            {
                TextHelper.Error($"PostPreOrderAsync 异常", ex);
                return DataResponse<string>.AsFail(message: ex.Message);
            }
        }

        #endregion


        #region 激活
        public async Task<DataResponse<bool>> ActiveAsync(string accessToken, string cardNo)
        {
            try
            {
                DoHandler(accessToken);
                var hkv = this.ToKeyValuePair().ToArray();
                return await PostAsync<DataResponse<bool>>($"{ConstDefintion.Pay_Url}/{cardNo}", hkvs: hkv);
            }
            catch (Exception ex)
            {
                TextHelper.Error($"ActiveAsync 异常", ex);
                return DataResponse<bool>.AsFail(message: ex.Message);
            }
        }
        #endregion

        #region 私有方法
        private List<KeyValuePair<string, string>> ToKeyValuePair()
        {
            var kv = new List<KeyValuePair<string, string>>();
            foreach (var item in dict)
            {
                var kvPair = new KeyValuePair<string, string>(item.Key, item.Value);
                kv.Add(kvPair);
            }
            return kv;
        }
        #endregion
    }
}
