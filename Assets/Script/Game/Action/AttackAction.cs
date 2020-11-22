using System;
using System.Collections.Generic;
using UnityEngine;

namespace hskim.Action {
    [ActionMapping(EActionType.Attack)]
    public class AttackAction : BaseAction { }

    public class AttackActionHandler : ActionHandler<AttackAction> {
        protected override IEnumerator<CustomYieldInstruction> Execute(StageContext context,
                                                                       AttackAction action) {
            throw new NotImplementedException();
        }
    }
}