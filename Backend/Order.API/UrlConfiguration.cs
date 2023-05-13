﻿namespace Order.API
{
    /// <summary>
    /// The configuration for the base url being used by the front ends and api.
    /// </summary>
    public class UrlConfiguration
    {
        /// <summary>
        /// The url of the api.
        /// </summary>
        public string Api { get; set; }
        /// <summary>
        /// The end user url.
        /// </summary>
        public string EndUser { get; set; }
        /// <summary>
        /// The admin url.
        /// </summary>
        public string Admin { get; set; }

        /// <summary>
        /// Instantiate a new instance of <see cref="UrlConfiguration"/>.
        /// </summary>
        public UrlConfiguration() {}

        /// <summary>
        /// Instantiate a new instance of <see cref="UrlConfiguration"/>.
        /// </summary>
        /// <param name="api">
        /// The url of the api.
        /// </param>
        /// <param name="endUser">
        /// The end user url.
        /// </param>
        /// <param name="admin">
        /// The admin url.
        /// </param>
        public UrlConfiguration(string api, string endUser, string admin)
        {
            Api = api;
            EndUser = endUser;
            Admin = admin;
        }
    }
}