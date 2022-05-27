/* Alert JS
   ========================================================================== */

class CloseAlert {
    constructor(alert) {
        this.alertClass = alert;
        this.closeIcon = alert.querySelector('[data-alert-close]');
      this.changeHeightClass = document.querySelector('.js-has-global-alert');
      this.init();
  }

  init() {
    const parentClassScope = this;
    this.closeIcon.addEventListener('click', function(e){
      parentClassScope.alertClass.remove();
      if (parentClassScope.changeHeightClass != null) {
          parentClassScope.changeHeightClass.classList.remove('js-has-global-alert');
      }
    });
  }
}

const closableAlerts = document.querySelectorAll('.c-global-alert');

if (closableAlerts.length) {
    [...closableAlerts].map((closableAlertsWithClick) => new CloseAlert(closableAlertsWithClick));
}
