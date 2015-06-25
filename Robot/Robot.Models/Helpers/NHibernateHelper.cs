using System;
using System.Diagnostics;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Caches.SysCache2;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Dialect;
using Robot.Models.DataObjects;

namespace Admissions.Models.Helpers
{
    internal static class NHibernateHelper
    {
        public static readonly ISessionFactory SessionFactory = null;

        static NHibernateHelper()
        {
            SessionFactory = Fluently
                .Configure()
                .Database
                (
                    MsSqlConfiguration
                        .MsSql2012
                            .ConnectionString(c => c.FromConnectionStringWithKey("default"))
                        .Dialect<MsSql2012Dialect>()
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Manufacturer>())
                .Cache(c => c.UseQueryCache().ProviderClass<SysCacheProvider>().UseSecondLevelCache()) 
                .ExposeConfiguration(x => x.SetProperty("current_session_context_class", "thread_static"))
                .ExposeConfiguration(x => x.SetProperty("show_sql", "false"))
                .ExposeConfiguration(x => x.SetProperty("connection.isolation", "ReadUncommitted"))
                .ExposeConfiguration(x => x.SessionFactory().Caching.WithDefaultExpiration(86400))
                .BuildConfiguration()
                .BuildSessionFactory();
        }

        public static ISession CurrentSession
        {
            get
            {
                if (CurrentSessionContext.HasBind(SessionFactory)) return SessionFactory.GetCurrentSession();
                var session = SessionFactory.OpenSession();
                
                CurrentSessionContext.Bind(session);

                return SessionFactory.GetCurrentSession();
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void CloseCurrentSession(bool ignoreException, bool hadPreviousException)
        {
            if (!CurrentSessionContext.HasBind(SessionFactory)) return;

            ISession session = null;

            try
            {
                session = CurrentSessionContext.Unbind(SessionFactory);

                if (session == null
                    || !session.IsOpen
                    || session.Transaction == null
                    || !session.Transaction.IsActive
                    || session.Transaction.WasCommitted
                    || session.Transaction.WasRolledBack) return;

                if (hadPreviousException)
                {
                    try
                    {
                        session.Transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        // Couldn't roll back the transaction, give up
                        Debug.Write(ex);
                    }
                }
                else
                {
                    try
                    {
                        session.Transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Failed to commit the transaction - try to roll it back
                        Debug.Write(ex);

                        try
                        {
                            session.Transaction.Rollback();
                        }
                        catch (TransactionException ex2)
                        {
                            // Couldn't roll back the transaction, give up
                            Debug.Write(ex2);
                        }

                        throw;
                    }
                }
            }
            catch (Exception)
            {
                if (ignoreException) return;

                throw;
            }
            finally
            {
                if (session != null)
                {
                    session.Dispose();
                }
            }
        }

        public static void FlushAndCommit()
        {
            CurrentSession.Flush();
            CurrentSession.Transaction.Commit();
        }

        public static void ClearQueryCache()
        {
            SessionFactory.EvictQueries();
            foreach (var collectionMetadata in SessionFactory.GetAllCollectionMetadata())
                SessionFactory.EvictCollection(collectionMetadata.Key);
            foreach (var classMetadata in SessionFactory.GetAllClassMetadata())
                SessionFactory.EvictEntity(classMetadata.Key);
        }
    }
}
