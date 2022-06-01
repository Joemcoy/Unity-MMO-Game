using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devdog.General
{
    public class PlayerTriggerHandler : PlayerTriggerHandlerBase<Collider>
    {
        private SphereCollider _collider;
        private Rigidbody _rigidbody;

        protected override void Awake()
        {
            base.Awake();

            _rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;

            _collider = gameObject.GetOrAddComponent<SphereCollider>();
            if (_collider)
            {
                _collider.isTrigger = true;
                _collider.radius = GeneralSettingsManager.instance.settings.triggerUseDistance;
            }
        }

        protected void OnTriggerEnter(Collider other)
        {
            NotifyTriggerEnter(other);
        }

        protected void OnTriggerExit(Collider other)
        {
            NotifyTriggerExit(other);
        }
    }
}