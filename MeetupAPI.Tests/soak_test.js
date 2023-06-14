// Long running tests to find memory leak,
// unexpected restarts, unallocated space, db connection exhaustion
import { sleep } from 'k6';
import http from 'k6/http';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '2m', target: 400 }, // ramp up to 400 users
        { duration: '3h56m', target: 400 }, // stay at 400 for ~4 hours
        { duration: '2m', target: 0 }, // scale down (optional)
    ],
};

const API_BASE_URL = 'https://localhost:5001';
const name = '4Dev';

export default () => {
    http.batch([
        ['GET', `${API_BASE_URL}/api/meetup?pageSize=50&pageNumber=1`],
        ['GET', `${API_BASE_URL}/api/meetup/${name}`],
    ]);

    sleep(1);
};