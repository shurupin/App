import simpleRestProvider from 'ra-data-simple-rest';
import { IS_REAL_DATA_PROVIDER_USED, REAL_API, FAKE_API } from '../constants/constants';

const url = IS_REAL_DATA_PROVIDER_USED ? REAL_API : FAKE_API;
const restProvider = simpleRestProvider(url);

const delayedDataProvider = new Proxy(restProvider, {
    get: (target, name, self) =>
        name === 'then' // as we await for the dataProvider, JS calls then on it. We must trap that call or else the dataProvider will be called with the then method
            ? self
            : (resource, params) =>
                  new Promise(resolve =>
                      setTimeout(
                          () => resolve(restProvider[name](resource, params)),
                          500
                      )
                  ),
});

export default delayedDataProvider;
