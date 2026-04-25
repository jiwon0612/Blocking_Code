using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public UnityEvent OnDeadEvent;
        
        private Dictionary<Type, IEntityCompo> _compos;
        public bool IsDead { get; set; }

        protected virtual void Awake()
        {
            _compos = new Dictionary<Type, IEntityCompo>();

            AddComponents();
            InitComponents();
            AfterInitCompos();
        }

        protected virtual void AddComponents()
        {
            GetComponentsInChildren<IEntityCompo>(true).ToList().ForEach(compo => _compos.Add(compo.GetType(), compo));
        }

        protected virtual void InitComponents()
        {
            IsDead = false;
            _compos.Values.ToList().ForEach(compo => compo.Initialize(this));
        }

        protected virtual void AfterInitCompos()
        {
            _compos.Values.OfType<IAfterInit>().ToList().ForEach(compo => compo.AfterInit());
        }

        public T GetCompo<T>(bool isDreived = false) where T : IEntityCompo
        {
            if (_compos.TryGetValue(typeof(T), out IEntityCompo compo))
                return (T)compo;
            
            if (!isDreived)
                return default;
            
            Type findType = _compos.Keys.FirstOrDefault(type => type.IsSubclassOf(typeof(T)));
            if (findType != null)
                return (T)_compos[findType];
            
            return default;
        }

        public virtual void OnDead()
        {
            OnDeadEvent?.Invoke();
            IsDead = true;
        }
    }
}