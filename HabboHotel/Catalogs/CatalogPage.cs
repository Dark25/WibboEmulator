﻿using Butterfly.Core;
using System.Collections.Generic;

namespace Butterfly.HabboHotel.Catalog
{
    public class CatalogPage
    {
        public int Id;

        public int ParentId;

        public bool Enabled;

        private readonly string Caption;
        private readonly string CaptionEn;
        private readonly string CaptionBr;

        public string PageLink;

        public int Icon;

        public int MinimumRank;

        public string Template;

        public List<string> PageStrings1 { get; private set; }

        public Dictionary<int, CatalogItem> Items { get; private set; }

        public Dictionary<int, CatalogItem> ItemOffers { get; private set; }

        private readonly List<string> PageStrings2;
        private readonly List<string> PageStrings2En;
        private readonly List<string> PageStrings2Br;

        public CatalogPage(int Id, int ParentId, string Enabled, string Caption, string PageLink, int Icon, int MinRank
            , string Template, string PageStrings1, string PageStrings2, string CaptionEn, string CaptionBr, string PageStrings2En, string PageStrings2Br, Dictionary<int, CatalogItem> Items)
        {
            this.Id = Id;
            this.ParentId = ParentId;
            this.Enabled = Enabled.ToLower() == "1" ? true : false;

            this.Caption = Caption;
            this.CaptionEn = CaptionEn;
            this.CaptionBr = CaptionBr;

            this.PageLink = PageLink;
            this.Icon = Icon;
            this.MinimumRank = MinRank;
            this.Template = Template;

            foreach (string Str in PageStrings1.Split('|'))
            {
                if (this.PageStrings1 == null) { this.PageStrings1 = new List<string>(); }
                this.PageStrings1.Add(Str);
            }

            foreach (string Str in PageStrings2.Split('|'))
            {
                if (this.PageStrings2 == null) { this.PageStrings2 = new List<string>(); }
                this.PageStrings2.Add(Str);
            }

            foreach (string Str in PageStrings2En.Split('|'))
            {
                if (this.PageStrings2En == null) { this.PageStrings2En = new List<string>(); }
                this.PageStrings2En.Add(Str);
            }

            foreach (string Str in PageStrings2Br.Split('|'))
            {
                if (this.PageStrings2Br == null) { this.PageStrings2Br = new List<string>(); }
                this.PageStrings2Br.Add(Str);
            }

            this.Items = Items;

            this.ItemOffers = new Dictionary<int, CatalogItem>();
            if (Template == "default_3x3")
            {
                foreach (CatalogItem item in this.Items.Values)
                {
                    if (item.IsLimited)
                    {
                        continue;
                    }

                    if (!this.ItemOffers.ContainsKey(item.Id))
                    {
                        this.ItemOffers.Add(item.Id, item);
                    }
                }
            }
        }

        public string GetCaptionByLangue(Language Langue)
        {
            if (Langue == Language.ANGLAIS)
            {
                return this.CaptionEn;
            }
            else if (Langue == Language.PORTUGAIS)
            {
                return this.CaptionBr;
            }

            return this.Caption;
        }

        public List<string> GetPageStrings2ByLangue(Language Langue)
        {
            if (Langue == Language.ANGLAIS)
            {
                return this.PageStrings2En;
            }
            else if (Langue == Language.PORTUGAIS)
            {
                return this.PageStrings2Br;
            }

            return this.PageStrings2;
        }

        public CatalogItem GetItem(int pId)
        {
            if (this.Items.ContainsKey(pId))
            {
                return this.Items[pId];
            }

            return null;
        }
    }
}