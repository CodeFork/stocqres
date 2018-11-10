﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Stocqres.Infrastructure.EventRepository.Scripts
{
    public static class EventRepositoryScriptsAsStrings
    {
        public static string GetAggregate(string aggregateName, Guid aggregateId) =>
            $"Select * From [Customers].{aggregateName}Events Where AggregateId='{aggregateId}'";

        public static string FindAggregateVersion(string aggregateName, Guid aggregateId) => 
            $"Select MAX(Version) FROM [Customers].{aggregateName}Events WHERE AggregateId='{aggregateId}'";

        public static string InsertIntoAggregate(string aggregateName) => 
            $"INSERT INTO [Customers].{aggregateName}Events(Id, AggregateId, AggregateType, Version, Data, Metadata, CreatedAt) " +
            "VALUES(@Id, @AggregateId, @AggregateType, @Version, @Data, @Metadata, @Created)";
        public static string CreateTableForAggregate(string aggregateName) =>
            $"IF NOT EXISTS(SELECT * FROM sysobjects  WHERE name = '{aggregateName}Events' AND xtype='U') " +
            $"CREATE TABLE [Customers].[{aggregateName}Events](" +
            "[Id] UNIQUEIDENTIFIER default NEWID() NOT NULL, " +
            "[AggregateId] UNIQUEIDENTIFIER NOT NULL, " +
            "[AggregateType] NVARCHAR(255) NOT NULL, " +
            "[Version] INT NOT NULL, " +
            "[Data] NVARCHAR(MAX) NOT NULL, " +
            "[MetaData] NVARCHAR(MAX) NOT NULL, " +
            "[CreatedAt] DATETIME NOT NULL, " +
            $"CONSTRAINT PK{aggregateName}Events PRIMARY KEY(ID) " +
            ") ";
        public static string CreateIndex(string aggregateName) =>
            $"IF NOT EXISTS(SELECT * FROM sys.indexes  WHERE name = 'Idx_{aggregateName}Events_AggregateId' AND object_id = OBJECT_ID('[Customers].{aggregateName}Events')) " +
            $"begin " +
            $"CREATE INDEX Idx_{aggregateName}Events_AggregateId ON [Customers].{aggregateName}Events(AggregateId)" +
            $"end";
    }
}
