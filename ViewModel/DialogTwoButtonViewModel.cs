using System;

namespace GuessWho.ViewModel {
    public class DialogYesNoViewModel : NedMaterialMVVM.ViewModel.DialogTwoButtonViewModel {
        public override string NameButton1 {
            get { return "TAK"; }
        }

        public override string NameButton2 {
            get { return "NIE"; }
        }

        public new void OpenDialog(string message, Action actionButton1 = null, Action actionButton2 = null) {
            base.OpenDialog(message, actionButton1, actionButton2);
        }

        public new void OpenDialog(object dialogIdentifier, string message, Action actionButton1 = null, Action actionButton2 = null) {
            base.OpenDialog(dialogIdentifier, message, actionButton1, actionButton2);
        }
    }
}
