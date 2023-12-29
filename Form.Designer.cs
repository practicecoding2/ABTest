using MergeListAutomation.Models;
using MergeListAutomation.Services;
using MergeListAutomationPoc.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MergeTool
{
    partial class Form
    {
        private static IConfiguration _iconfiguration;
        private static string _token;
        private static string _baseDevOpsApiUrl;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblReleaseVersion = new System.Windows.Forms.Label();
            this.btnUpdateRelease = new System.Windows.Forms.Button();
            this.btnMergeList = new System.Windows.Forms.Button();
            this.dtpReleaseVersion = new System.Windows.Forms.DateTimePicker();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblReleaseVersion
            // 
            this.lblReleaseVersion.AutoSize = true;
            this.lblReleaseVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReleaseVersion.ForeColor = System.Drawing.SystemColors.Desktop;
            this.lblReleaseVersion.Location = new System.Drawing.Point(21, 25);
            this.lblReleaseVersion.Name = "lblReleaseVersion";
            this.lblReleaseVersion.Size = new System.Drawing.Size(147, 20);
            this.lblReleaseVersion.TabIndex = 1;
            this.lblReleaseVersion.Text = "Release Version";
            // 
            // btnUpdateRelease
            // 
            this.btnUpdateRelease.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnUpdateRelease.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnUpdateRelease.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateRelease.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnUpdateRelease.Location = new System.Drawing.Point(25, 109);
            this.btnUpdateRelease.Name = "btnUpdateRelease";
            this.btnUpdateRelease.Size = new System.Drawing.Size(200, 50);
            this.btnUpdateRelease.TabIndex = 3;
            this.btnUpdateRelease.Text = "Update Release #";
            this.btnUpdateRelease.UseVisualStyleBackColor = false;
            this.btnUpdateRelease.Click += new System.EventHandler(this.btnUpdateRelease_Click);
            // 
            // btnMergeList
            // 
            this.btnMergeList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnMergeList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMergeList.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnMergeList.Location = new System.Drawing.Point(266, 109);
            this.btnMergeList.Name = "btnMergeList";
            this.btnMergeList.Size = new System.Drawing.Size(200, 50);
            this.btnMergeList.TabIndex = 4;
            this.btnMergeList.Text = "Get Merge List";
            this.btnMergeList.UseVisualStyleBackColor = false;
            this.btnMergeList.Click += new System.EventHandler(this.btnMergeList_Click);
            // 
            // dtpReleaseVersion
            // 
            this.dtpReleaseVersion.CustomFormat = "yyyy.MM.dd";
            this.dtpReleaseVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpReleaseVersion.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpReleaseVersion.Location = new System.Drawing.Point(25, 50);
            this.dtpReleaseVersion.MinDate = new System.DateTime(2022, 9, 9, 0, 0, 0, 0);
            this.dtpReleaseVersion.Name = "dtpReleaseVersion";
            this.dtpReleaseVersion.Size = new System.Drawing.Size(200, 28);
            this.dtpReleaseVersion.TabIndex = 5;
            // 
            // txtOutput
            // 
            this.txtOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(25, 184);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(441, 181);
            this.txtOutput.TabIndex = 6;
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(495, 387);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.dtpReleaseVersion);
            this.Controls.Add(this.btnMergeList);
            this.Controls.Add(this.btnUpdateRelease);
            this.Controls.Add(this.lblReleaseVersion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Merge Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblReleaseVersion;
        private System.Windows.Forms.Button btnUpdateRelease;
        private System.Windows.Forms.Button btnMergeList;
        private System.Windows.Forms.DateTimePicker dtpReleaseVersion;
        private System.Windows.Forms.TextBox txtOutput;


        #region Main Logic

        //Load config settings
        private static void LoadConfig()
        {
            _iconfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            _token = _iconfiguration["PersonalAccessToken"];
            _baseDevOpsApiUrl = _iconfiguration["BaseDevOpsApiUrl"];
        }

        //Print output for info purposes
        private void PrintOutput(string message)
        {
            txtOutput.AppendText($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] - {message}{Environment.NewLine}");
        }

        //Set default release date
        public void SetDefaultReleaseDate()
        {
            dtpReleaseVersion.Text = DateTime.Today.ToString("yyyy.MM.dd");
        }

        //Run UpdateReleaseVersionNumber pipeline
        private void UpdateReleaseVersionNumber()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{string.Empty}:{_token}")));

                    //Set request data, defaults to upcoming Thursday
                    var releaseVersionipeLineId = _iconfiguration["UpdateReleaseVersionNumbersPipelineId"];
                    var pipelineReq = new PipelineRun
                    {
                        previewRun = false,
                        variables = new Dictionary<string, Variable>()
                    };
                    pipelineReq.variables.Add("Definitions", new Variable { value = "Release" });
                    pipelineReq.variables.Add("system.debug", new Variable { value = "false" });
                    pipelineReq.variables.Add("BuildDefinitionName", new Variable { value = "*" });
                    pipelineReq.variables.Add("NewVersionNumber", new Variable { value = dtpReleaseVersion.Text });
                    var content = new StringContent(JsonConvert.SerializeObject(pipelineReq), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = client.PostAsync(
                        $"{_baseDevOpsApiUrl}/pipelines/{releaseVersionipeLineId}/runs?api-version=6.0-preview.1", content).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        PrintOutput("Release version update SUCCESS");
                    }
                }
            }
            catch (Exception ex)
            {
                PrintOutput("Release version update FAILED");
            }
        }

        //Get final merge list of repos worked on in iteration
        public List<Value> GetMergeList()
        {
            List<Value> finalMergeList = new List<Value>();
            var repoList = GetRepositoryList();

            try
            {
                foreach (var repo in repoList.value)
                {
                    //If repo not in exclusion list, continue to check latest commits
                    if (!ExcludeList.repoList.Contains(repo.name)
                        && !repo.name.StartsWith("VPS", StringComparison.InvariantCultureIgnoreCase))
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization =
                               new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{string.Empty}:{_token}")));

                            using (HttpResponseMessage response = client.GetAsync($"{_baseDevOpsApiUrl}/git/repositories/{repo.id}/commits").Result)
                            {
                                response.EnsureSuccessStatusCode();                                
                                var responseBody = JsonConvert.DeserializeObject<CommitResult>(response.Content.ReadAsStringAsync().Result);

                                if (responseBody.value.Count > 0)
                                {
                                    DateTime iterationStart = DateTime.Today.AddDays(-11);
                                    DateTime iterationEnd = DateTime.Today.AddDays(1);
                                    DateTime lastUpdatedDate = DateTime.Parse(responseBody.value[0].author.date);

                                    if (iterationStart < lastUpdatedDate
                                        && lastUpdatedDate < iterationEnd)
                                    {
                                        finalMergeList.Add(repo);
                                    }
                                }

                            }
                        }
                    }
                }
                PrintOutput("Repo commit check SUCCESS");
            }
            catch (Exception ex)
            {
                PrintOutput("Repo commit check FAILED");
                throw ex;
            }
            return finalMergeList;
        }

        //Get repository list
        public RepoResult GetRepositoryList()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{string.Empty}:{_token}")));

                    using (HttpResponseMessage response = client.GetAsync($"{_baseDevOpsApiUrl}/git/repositories").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        PrintOutput("Get repo list SUCCESS");
                        return JsonConvert.DeserializeObject<RepoResult>(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintOutput("Get repo list FAILED");
                throw ex;
            }
        }

        //Send merge list email
        private void SendMergeListEmail(List<Value> mergeList)
        {
            List<string> repoNameList = mergeList.Select(element => element.name).ToList();
            repoNameList.Sort();

            StringBuilder sb = new StringBuilder(string.Empty);
            sb.Append("<table>");
            sb.Append("<tr>");
            sb.Append("<td><b>Repo Name</b>&emsp;</td>");
            sb.Append("</tr>");

            for (int i = 0; i < repoNameList.Count; i++)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + repoNameList[i] + "&emsp;</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");

            var email = new Email
            {
                FromEmail = _iconfiguration["FromEmail"],
                ToEmail = _iconfiguration["ToEmail"],
                Subject = $"{DateTime.Today.Date.ToString("MM-dd-yyyy")} Merge List",
                Message = $"<html><body>\r\n<br/><br/>The following repositories need to be merged today: <br/><br/>{sb}"
            };
            SendEmailService(email);
        }

        public void SendEmailService(Email email)
        {
            if (email != null)
            {
                try
                {
                    //Call PS API to send email
                    var fullApiUrl = $"{_iconfiguration["BasePSApiUrl"]}/common/common/SendEmail?subscription-key={_iconfiguration["PSApiAccessKey"]}";
                    var jsonData = JsonConvert.SerializeObject(email);
                    ServiceCall.PostWebApiCall<bool>(fullApiUrl, jsonData);
                    PrintOutput("Sending email SUCCESS");
                }
                catch (Exception ex)
                {
                    PrintOutput("Sending email FAILED");
                    throw ex;
                }
            }
        }

        #endregion Main Logic
    }
}

