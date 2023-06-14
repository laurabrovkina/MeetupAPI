import { sleep } from 'k6';
import http from 'k6/http';

// these options are needed because the tests are running locally
export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    vus: 1,
    duration: '10s'
};

export default () => {
    http.get('https://localhost:5001/api/meetup?pageSize=50&pageNumber=1');
    sleep(1);
};