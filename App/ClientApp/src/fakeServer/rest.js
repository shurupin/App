import FakeRest from 'fakerest';
import fetchMock from 'fetch-mock';
import generateData from 'data-generator-retail';
import { FAKE_API } from '../constants/constants';

export default () => {
    const data = generateData({ serializeDate: true });
    const restServer = new FakeRest.FetchServer(FAKE_API);
    if (window) {
        window.restServer = restServer; // give way to update data in the console
    }
    restServer.init(data);
    restServer.toggleLogging(); // logging is off by default, enable it
    fetchMock.mock(`begin:${FAKE_API}`, restServer.getHandler());
    return () => fetchMock.restore();
};
