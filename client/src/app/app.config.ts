import { APP_INITIALIZER, ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorInterceptor } from './Core/interceptor/error.interceptor';
import { loadingInterceptor } from './Core/interceptors/loading.interceptor';
import { InitService } from './Core/services/init.service';
import { lastValueFrom } from 'rxjs';

function initApp(initService: InitService){
  return() => lastValueFrom(initService.init()).finally(() => {
    const splash = document.getElementById('initial-splash');
    if(splash){
      splash.remove();
    }
  })
}

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes), 
    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([errorInterceptor, loadingInterceptor])),
    {
      provide: APP_INITIALIZER,
      useFactory: initApp,
      multi: true,
      deps: [InitService]
    }
  ],
};
