// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Storage.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InMemoryStore : IInMemoryStore
    {
        private readonly IInMemoryTableFactory _tableFactory;
        private readonly bool _useNameMatching;

        private readonly object _lock = new object();

        private LazyRef<Dictionary<object, IInMemoryTable>> _tables = CreateTables();

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public InMemoryStore([NotNull] IInMemoryTableFactory tableFactory)
            : this(tableFactory, useNameMatching: false)
        {
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public InMemoryStore(
            [NotNull] IInMemoryTableFactory tableFactory,
            bool useNameMatching)
        {
            _tableFactory = tableFactory;
            _useNameMatching = useNameMatching;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual bool EnsureCreated(IModel model)
        {
            lock (_lock)
            {
                var returnValue = !_tables.HasValue;

                // ReSharper disable once AssignmentIsFullyDiscarded
                _ = _tables.Value;

                return returnValue;
            }
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual bool Clear()
        {
            lock (_lock)
            {
                if (!_tables.HasValue)
                {
                    return false;
                }

                _tables = CreateTables();

                return true;
            }
        }

        private static LazyRef<Dictionary<object, IInMemoryTable>> CreateTables()
            => new LazyRef<Dictionary<object, IInMemoryTable>>(() => new Dictionary<object, IInMemoryTable>());

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual IReadOnlyList<InMemoryTableSnapshot> GetTables(IEntityType entityType)
        {
            var data = new List<InMemoryTableSnapshot>();
            lock (_lock)
            {
                if (_tables.HasValue)
                {
                    foreach (var et in entityType.GetConcreteTypesInHierarchy())
                    {
                        var key = _useNameMatching ? (object)et.Name : et;
                        if (_tables.Value.TryGetValue(key, out var table))
                        {
                            data.Add(new InMemoryTableSnapshot(et, table.SnapshotRows()));
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual int ExecuteTransaction(
            IEnumerable<IUpdateEntry> entries,
            IDiagnosticsLogger<DbLoggerCategory.Update> updateLogger)
        {
            var rowsAffected = 0;

            lock (_lock)
            {
                foreach (var entry in entries)
                {
                    var entityType = entry.EntityType;

                    Debug.Assert(!entityType.IsAbstract());

                    var key = _useNameMatching ? (object)entityType.Name : entityType;
                    if (!_tables.Value.TryGetValue(key, out var table))
                    {
                        _tables.Value.Add(key, table = _tableFactory.Create(entityType));
                    }

                    if (entry.SharedIdentityEntry != null)
                    {
                        if (entry.EntityState == EntityState.Deleted)
                        {
                            continue;
                        }

                        table.Delete(entry);
                    }

                    switch (entry.EntityState)
                    {
                        case EntityState.Added:
                            table.Create(entry);
                            break;
                        case EntityState.Deleted:
                            table.Delete(entry);
                            break;
                        case EntityState.Modified:
                            table.Update(entry);
                            break;
                    }

                    rowsAffected++;
                }
            }

            updateLogger.ChangesSaved(entries, rowsAffected);

            return rowsAffected;
        }
    }
}
